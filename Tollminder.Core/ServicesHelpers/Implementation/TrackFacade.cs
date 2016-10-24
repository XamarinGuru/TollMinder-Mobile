using System;
using System.Collections.Generic;
using System.Threading;
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
        SemaphoreSlim _semaphor;

        List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();
        MvxSubscriptionToken _locationToken;
        MvxSubscriptionToken _motionToken;

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

            _semaphor = new SemaphoreSlim(1);

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
                _activity.StartDetection();
                _geoWatcher.StartGeolocationWatcher();
                _locationToken = _messenger.SubscribeOnThreadPoolThread<LocationMessage>(x =>
               {
                   Log.LogMessage("Start processing LocationMessage");
                   CheckTrackStatus();
               });
                _tokens.Add(_locationToken);

               _motionToken = _messenger.SubscribeOnThreadPoolThread<MotionMessage>(async x =>
               {
                   Log.LogMessage($"[FACADE] receive new motion type {x.Data}");
                   switch (x.Data)
                   {
                       case MotionType.Automotive:
                       case MotionType.Running:
                       case MotionType.Walking:
                           if ((_storedSettingsService.SleepGPSDateTime == DateTime.MinValue || _storedSettingsService.SleepGPSDateTime < DateTime.Now)
                               && !IsBound)
                           {
                               Log.LogMessage($"[FACADE] Start geolocating because we are not still");
                               await StartServices();
                           }
                           break;
                       case MotionType.Still:
                           if (IsBound)
                           {
                               Log.LogMessage($"[FACADE] Stop geolocating because we are still");
                               StopServices();
                           }
                           break;
                   }
               });
                _tokens.Add(_motionToken);
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
            _tokens.Remove(_locationToken);
            _tokens.Remove(_motionToken);
            _motionToken?.Dispose();

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

        protected async virtual void CheckTrackStatus()
        {
            if (_locationProcessing)
            {
                Log.LogMessage("Ignore location in FACADE because location processing");
                return;
            }

            await _semaphor.WaitAsync();

            if (_locationProcessing)
                return;

            _locationProcessing = true;

            try
            {
                BaseStatus statusObject = StatusesFactory.GetStatus(TollStatus);

                if (_activity.MotionType == MotionType.Still)
                {
                    Log.LogMessage("Ignore location in FACADE because we are still");
                    return;
                }
                else
                {
                    if (statusObject.CheckBatteryDrain())
                    {
                        Log.LogMessage("Ignore location in FACADE because we are too away from nearest waypoint");
                        return;
                    }
                }

                var statusBeforeCheck = TollStatus;
                Log.LogMessage($"Current status before check= {TollStatus}");

                TollStatus = await statusObject.CheckStatus();

                statusObject = StatusesFactory.GetStatus(TollStatus);

                Log.LogMessage($"Current status after check = {TollStatus}");
                if (statusBeforeCheck != TollStatus)
                    Mvx.Resolve<INotificationSender>().SendLocalNotification($"Status: {TollStatus.ToString()}", $"Lat: {_geoWatcher.Location?.Latitude}, Long: {_geoWatcher.Location?.Longitude}");
            }
            catch (Exception e)
            {
                Log.LogMessage(e.Message + e.StackTrace);
            }
            finally
            {
                _locationProcessing = false;
                _semaphor.Release();
            }
        }
    }
}