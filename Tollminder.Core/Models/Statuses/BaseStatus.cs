using System;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.GeoData;
using Tollminder.Core.Services.Notifications;
using Tollminder.Core.Services.RoadsProcessing;
using Tollminder.Core.Services.Settings;
using Tollminder.Core.Services.SpeechRecognition;
using System.Collections.Generic;
using Tollminder.Core.Models.GeoData;

namespace Tollminder.Core.Models.Statuses
{
    public abstract class BaseStatus
    {
        private int firstElement = 0;
        private TollGeolocationStatus tollGeolocationStatus;

        IGeoLocationWatcher _geoWatcher;
        protected IGeoLocationWatcher GeoWatcher
        {
            get
            {
                return _geoWatcher ?? (_geoWatcher = Mvx.Resolve<IGeoLocationWatcher>());
            }
        }

        IMotionActivity _motionActivity;
        protected IMotionActivity MotionActivity
        {
            get
            {
                return _motionActivity ?? (_motionActivity = Mvx.Resolve<IMotionActivity>());
            }
        }

        IDistanceChecker _distanceChecker;
        protected IDistanceChecker DistanceChecker
        {
            get
            {
                return _distanceChecker ?? (_distanceChecker = Mvx.Resolve<IDistanceChecker>());
            }
        }

        IWaypointChecker _waypointChecker;
        protected IWaypointChecker WaypointChecker
        {
            get
            {
                if (_waypointChecker == null)
                {
                    _waypointChecker = Mvx.Resolve<IWaypointChecker>();
                }
                return _waypointChecker;
            }
        }

        IGeoDataService _dataService;
        protected IGeoDataService GeoDataService
        {
            get
            {
                return _dataService ?? (_dataService = Mvx.Resolve<IGeoDataService>());
            }
        }

        INotifyService _notifyService;
        protected INotifyService NotifyService
        {
            get
            {
                return _notifyService ?? (_notifyService = Mvx.Resolve<INotifyService>());
            }
        }

        ISpeechToTextService _speechToTextService;
        public ISpeechToTextService SpeechToTextService
        {
            get
            {
                return _speechToTextService ?? (_speechToTextService = Mvx.Resolve<ISpeechToTextService>());
            }
        }

        IBatteryDrainService _batteryDrainService;
        protected IBatteryDrainService BatteryDrainService
        {
            get
            {
                return _batteryDrainService ?? (_batteryDrainService = Mvx.Resolve<IBatteryDrainService>());
            }
        }

        protected Task<TollGeoStatusResult> CheckNearestPoint(TollGeolocationStatus tollGeoStatus, List<TollPointWithDistance> tollPoints = null)
        {
            Log.LogMessage(string.Format($"TRY TO FIND TOLLPOINT EXITS FROM {SettingsService.WaypointLargeRadius * 1000} m"));

            var location = GeoWatcher.Location;
            var nearestWaypoints = GeoDataService.FindNearestTollPoints(location);//tollPoints != null ? tollPoints : GeoDataService.FindNearestTollPoints(location);

            WaypointChecker.SetTollPointsInRadius(nearestWaypoints);

            WaypointChecker.SetIgnoredChoiceTollPoint(null);

            foreach (var item in WaypointChecker.TollPointsInRadius)
                Log.LogMessage($"FOUNDED WAYPOINT : {item.Name}, DISTANCE {item.Distance}");

            GeoWatcher.StartUpdatingHighAccuracyLocation();
            switch (WaypointChecker.TollPointsInRadius[firstElement].WaypointAction)
            {
                case WaypointAction.Entrance:
                    if (tollGeoStatus == TollGeolocationStatus.OnTollRoad)
                        tollGeolocationStatus = TollGeolocationStatus.OnTollRoad;
                    else
                        tollGeolocationStatus = TollGeolocationStatus.NearTollRoadEntrance;
                    break;
                case WaypointAction.Bridge:
                    tollGeolocationStatus = TollGeolocationStatus.NearTollRoadEntrance;
                    break;
                case WaypointAction.Exit:
                    if (tollGeoStatus != TollGeolocationStatus.NotOnTollRoad)
                        tollGeolocationStatus = TollGeolocationStatus.NearTollRoadExit;
                    break;
            }

            return Task.FromResult(new TollGeoStatusResult()
            {
                TollGeolocationStatus = tollGeolocationStatus,
                IsNeedToDoubleCheck = false
            });
        }

        public abstract Task<TollGeoStatusResult> CheckStatus(TollGeolocationStatus tollGeoStatus);
        public abstract bool CheckBatteryDrain();
    }
}

