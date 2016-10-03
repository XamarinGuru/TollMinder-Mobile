using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Models.Statuses;
using Tollminder.Core.Services;

namespace Tollminder.Core.ServicesHelpers.Implementation
{
    public class TrackFacade : ITrackFacade
    {
        #region Services

        readonly IGeoLocationWatcher _geoWatcher;
        readonly IMotionActivity _activity;
        readonly ITextToSpeechService _textToSpeech;
        readonly IMvxMessenger _messenger;
        readonly IBatteryDrainService _batteryDrainService;
        readonly ISpeechToTextService _speechToTextService;
        readonly IStoredSettingsService _storedSettingsService;

        object _locker = new object();
        bool _locationProcessing;

        List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();
        MvxSubscriptionToken _token;

        #endregion

        #region Constructor

        public TrackFacade()
        {
            Log.LogMessage("Facade init start");
            _textToSpeech = Mvx.Resolve<ITextToSpeechService>();
            _messenger = Mvx.Resolve<IMvxMessenger>();
            _geoWatcher = Mvx.Resolve<IGeoLocationWatcher>();
            _activity = Mvx.Resolve<IMotionActivity>();
            _batteryDrainService = Mvx.Resolve<IBatteryDrainService>();
            _speechToTextService = Mvx.Resolve<ISpeechToTextService>();
            _storedSettingsService = Mvx.Resolve<IStoredSettingsService>();

            if (_storedSettingsService.SleepGPSDateTime != DateTime.MinValue
                      && _storedSettingsService.SleepGPSDateTime > DateTime.Now
                      && _batteryDrainService.CheckGpsTrackingSleepTime(TollStatus))
            {
                Log.LogMessage("Relaunch BatteryDrainService");
                return;
            }

            if (_storedSettingsService.GeoWatcherIsRunning)
            {
                Task.Run(async () =>
                {
                    StopServices();

                    await StartServices().ConfigureAwait(false);
                    Log.LogMessage("Autostart facade");
                });
            }

            //_tokens.Add(_messenger.SubscribeOnThreadPoolThread<MotionMessage>(async x =>
            //{
            //    switch (x.Data)
            //    {
            //        case MotionType.Automotive:
            //        case MotionType.Running:
            //        case MotionType.Walking:
            //            if (!IsBound)
            //                await StartServices().ConfigureAwait(false);
            //            break;
            //        default:
            //            if (IsBound)
            //                StopServices();
            //            break;
            //    }
            //}));
            Log.LogMessage("Facade init end");
        }

        #endregion

        #region Properties

        bool IsBound
        {
            get
            {
                return _geoWatcher.IsBound;
            }
        }

        #endregion

        public virtual async Task<bool> StartServices()
        {
            bool isGranted = await Mvx.Resolve<IPermissionsService>().CheckPermissionsAccesGrantedAsync();
            if (!IsBound && isGranted)
            {

                Log.LogMessage(string.Format("FACADE HAS STARTED AT {0}", DateTime.Now));

                _textToSpeech.IsEnabled = true;
                _geoWatcher.StartGeolocationWatcher();
                _token = _messenger.SubscribeOnThreadPoolThread<LocationMessage>(x =>
               {
                   Log.LogMessage("Facade received LocationMessage");
                   if (!_locationProcessing)
                   {
                       Log.LogMessage("Start processing LocationMessage");
                       CheckTrackStatus();
                   }
               });
                _tokens.Add(_token);
                _activity.StartDetection();
                Log.LogMessage("Start Facade location detection and subscride on LocationMessage");
                return true;
            }

            return false;
        }

        public virtual bool StopServices()
        {
            if (!IsBound)
                return false;

            Log.LogMessage(string.Format("FACADE HAS STOPPED AT {0}", DateTime.Now));

            _geoWatcher.StopGeolocationWatcher();
            _tokens.Remove(_token);
            _token?.Dispose();

            return true;
        }

        public TollGeolocationStatus TollStatus
        {
            get
            {
                return _storedSettingsService.Status;
            }
            set
            {
                _storedSettingsService.Status = value;
                _messenger.Publish(new StatusMessage(this, value));
            }
        }

        protected virtual void CheckTrackStatus()
        {
            lock (_locker)
            {
                if (_locationProcessing)
                    return;

                _locationProcessing = true;

                try
                {
                    if (_batteryDrainService.CheckGpsTrackingSleepTime(TollStatus))
                        return;

                    BaseStatus statusObject = StatusesFactory.GetStatus(TollStatus);

                    Log.LogMessage($"Current status before check= {TollStatus}");

                    var task = statusObject.CheckStatus();

                    Task.WaitAny(task);

                    if (task.IsFaulted)
                    {
                        Mvx.Trace(task.Exception.Message + task.Exception.StackTrace + task.Exception.InnerException?.Message + task.Exception.InnerException?.StackTrace);
                        return;
                    }

                    TollStatus = task.Result;

                    statusObject = StatusesFactory.GetStatus(TollStatus);

                    Log.LogMessage($"Current status after check = {TollStatus}");
                    Mvx.Resolve<INotificationSender>().SendLocalNotification($"Status: {TollStatus.ToString()}", $"Lat: {_geoWatcher.Location?.Latitude}, Long: {_geoWatcher.Location?.Longitude}");
                }
                catch (Exception e)
                {
                    Log.LogMessage(e.Message + e.StackTrace);
                }
                finally
                {
                    _locationProcessing = false;
                }
            }
        }
    }
}