using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Models.Statuses;
using System.Diagnostics;
using Xamarin;
using Tollminder.Core.Services.GeoData;
using Tollminder.Core.Services.RoadsProcessing;
using Tollminder.Core.Services.SpeechRecognition;
using Tollminder.Core.Services.Settings;
using Tollminder.Core.Services.Notifications;
using Tollminder.Core.Models.GeoData;

namespace Tollminder.Core.ServicesHelpers
{
    public class TrackFacade : ITrackFacade
    {
        #region Services

        readonly IGeoLocationWatcher _geoWatcher;
        readonly IMotionActivity _activity;
        readonly ITextToSpeechService _textToSpeech;
        readonly IMvxMessenger _messenger;
        readonly IStoredSettingsService _storedSettingsService;
        readonly IWaypointChecker waypointChecker;

        bool _locationProcessing;
        SemaphoreSlim _semaphor;

        List<MvxSubscriptionToken> _tokens;
        MvxSubscriptionToken _locationToken;
        MvxSubscriptionToken _motionToken;
        private TollGeoStatusResult doubleCheckResult;

        #endregion

        #region Constructor

        public TrackFacade(ITextToSpeechService textToSpeech, IMvxMessenger messenger, IGeoLocationWatcher geoWatcher,
                           IMotionActivity activity, IStoredSettingsService storedSettingsService, IWaypointChecker waypointChecker)
        {
            Log.LogMessage("Facade ctor start");
            _textToSpeech = textToSpeech;
            _messenger = messenger;
            _geoWatcher = geoWatcher;
            _activity = activity;
            _storedSettingsService = storedSettingsService;
            this.waypointChecker = waypointChecker;

            _tokens = new List<MvxSubscriptionToken>(); // was changed
            _semaphor = new SemaphoreSlim(1);
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
                              await StartServicesAsync();
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
            Log.LogMessage("Facade ctor end");
        }

        #endregion

        #region Properties

        bool IsBound
        {
            get { return _geoWatcher.IsBound; }
        }

        #endregion

        public virtual async Task<bool> StartServicesAsync()
        {
            bool isGranted = await Mvx.Resolve<IPermissionsService>().CheckPermissionsAccesGrantedAsync();
            if (!IsBound && isGranted)
            {
                Log.LogMessage(string.Format("FACADE HAS STARTED AT {0}", DateTime.Now));
                Debug.WriteLine("Inside tracking service");

                _textToSpeech.IsEnabled = true;
                _activity.StartDetection();
                _geoWatcher.StartGeolocationWatcher();
                Insights.Track("Subscribed on LocationMessage.");
                _locationToken = _messenger.SubscribeOnThreadPoolThread<LocationMessage>(async x =>
               {
                   Log.LogMessage("Start processing LocationMessage");
                   await CheckTrackStatusAsync();
               });
                _tokens.Add(_locationToken);

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

            return true;
        }

        public TollGeolocationStatus TollStatus
        {
            get { return _storedSettingsService.Status; }
            set
            {
                _storedSettingsService.Status = value;
                _messenger.Publish(new StatusMessage(this, value));
            }
        }

        protected async virtual Task CheckTrackStatusAsync()
        {
            Log.LogMessage("Track status is cheking...");

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

                var checkResult = await StatusesFactory.GetStatus(TollGeolocationStatus.NearestTollPoint)
                                                       .CheckStatus(new TollGeoStatusResult() { TollGeolocationStatus = statusBeforeCheck });//await statusObject.CheckStatus(statusBeforeCheck);
                TollStatus = checkResult.TollGeolocationStatus;

                statusObject = StatusesFactory.GetStatus(TollStatus);

                Log.LogMessage($"Current status after check = {TollStatus}");
                if (statusBeforeCheck != TollStatus)
                {
                    Mvx.Resolve<INotificationSender>().SendLocalNotification($"Status: {TollStatus.ToString()}", $"Lat: {_geoWatcher.Location?.Latitude}, Long: {_geoWatcher.Location?.Longitude}");
                }
            }
            catch (Exception e)
            {
                Insights.Report(e);
                Log.LogMessage(e.Message + e.StackTrace);
            }
            finally
            {
                _locationProcessing = false;
                _semaphor.Release();
            }
        }

        public async Task CheckAreWeStillOnTheRoadAsync()
        {
            if (waypointChecker.TollPoint != null)
            {
                Debug.WriteLine(waypointChecker.TollPoint.Name);
                BaseStatus statusObject = StatusesFactory.GetStatus(TollStatus);
                _textToSpeech.IsEnabled = true;

                switch (TollStatus)
                {
                    case TollGeolocationStatus.NearTollRoadEntrance:
                    case TollGeolocationStatus.NotOnTollRoad:
                        break;
                    case TollGeolocationStatus.NearTollRoadExit:
                    case TollGeolocationStatus.OnTollRoad:
                        if (await statusObject.SpeechToTextService.AskQuestionAsync($"Are you still going from {waypointChecker.TollPoint.Name} tollroad?"))
                        {
                            waypointChecker.SetEntrance(waypointChecker.TollPoint);

                            if (waypointChecker.TollPoint.WaypointAction == WaypointAction.Bridge)
                            {
                                waypointChecker.SetExit(waypointChecker.TollPoint);

                                waypointChecker.SetTollPointsInRadius(null);
                                waypointChecker.ClearData();
                                TollStatus = TollGeolocationStatus.NotOnTollRoad;
                            }
                            else
                                TollStatus = TollGeolocationStatus.OnTollRoad;
                        }
                        else
                        {
                            TollStatus = TollGeolocationStatus.NotOnTollRoad;
                        }
                        break;
                }
            }
        }

        public async Task InitializeAsync()
        {
            if (_storedSettingsService.GeoWatcherIsRunning)
            {
                StopServices();
                await StartServicesAsync().ConfigureAwait(false);
                Log.LogMessage("Autostart facade");
            }
        }
    }
}