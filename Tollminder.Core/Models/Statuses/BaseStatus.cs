﻿using System;
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
        private bool shouldContinueCheckStatus = true;

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
            var location = GeoWatcher.Location;
            var nearestWaypoints = GeoDataService.FindNearestTollPoints(location);

            if (nearestWaypoints?.Count == 0)
            {
#if REALEASE
                GeoWatcher.StopUpdatingHighAccuracyLocation();
#endif
                Log.LogMessage($"No waypoint founded for location {GeoWatcher.Location}");
                SetTollStatusWithDistance(location, Mvx.Resolve<IStoredSettingsService>().CurrentRoadStatus == TollGeolocationStatus.OnTollRoad
                                                       ? Mvx.Resolve<IStoredSettingsService>().CurrentRoadStatus
                                                       : TollGeolocationStatus.NotOnTollRoad);
                shouldContinueCheckStatus = false;
                return Task.FromResult(new TollGeoStatusResult()
                {
                    TollPointWithDistance = null,
                    Location = location,
                    TollGeolocationStatus = tollGeolocationStatus,
                    IsNeedToDoubleCheck = shouldContinueCheckStatus
                });
            }

            WaypointChecker.SetTollPointsInRadius(nearestWaypoints);
            WaypointChecker.SetIgnoredChoiceTollPoint(null);

            var tollPointInRadius = WaypointChecker.TollPointsInRadius[firstElement];

            foreach (var item in WaypointChecker.TollPointsInRadius)
                Log.LogMessage($"FOUNDED WAYPOINT : {item.Name}, DISTANCE {item.Distance}");
#if REALEASE
            GeoWatcher.StartUpdatingHighAccuracyLocation();
#endif
            switch (tollPointInRadius.WaypointAction)
            {
                case WaypointAction.Entrance:
                    if (tollGeoStatus == TollGeolocationStatus.OnTollRoad)
                    {
                        tollGeolocationStatus = TollGeolocationStatus.OnTollRoad;
                        shouldContinueCheckStatus = false;
                    }
                    else if (Mvx.Resolve<IStoredSettingsService>().CurrentRoadStatus == TollGeolocationStatus.NotOnTollRoad
                        || Mvx.Resolve<IStoredSettingsService>().CurrentRoadStatus == TollGeolocationStatus.SearchingNearestTollPoint)
                    {
                        SetTollStatusWithDistance(location, TollGeolocationStatus.NearTollRoadEntrance);
                    }
                    break;
                case WaypointAction.Bridge:
                    SetTollStatusWithDistance(location, TollGeolocationStatus.NearTollRoadEntrance);
                    break;
                case WaypointAction.Exit:
                    if (Mvx.Resolve<IStoredSettingsService>().CurrentRoadStatus == TollGeolocationStatus.OnTollRoad)
                        SetTollStatusWithDistance(location, TollGeolocationStatus.NearTollRoadExit);
                    else
                    {
                        tollGeolocationStatus = tollGeoStatus;
                        shouldContinueCheckStatus = false;
                    }
                    break;
            }

            return Task.FromResult(new TollGeoStatusResult()
            {
                TollPointWithDistance = tollPointInRadius,
                Location = location,
                TollGeolocationStatus = tollGeolocationStatus,
                IsNeedToDoubleCheck = shouldContinueCheckStatus
            });
        }

        private void SetTollStatusWithDistance(GeoLocation location, TollGeolocationStatus tollStatus)
        {
            WaypointChecker.DetectWeAreInsideSomeTollPoint(location);
            tollGeolocationStatus = tollStatus;
            shouldContinueCheckStatus = true;
        }

        public abstract Task<TollGeoStatusResult> CheckStatus(TollGeoStatusResult tollGeoStatus);
        public abstract bool CheckBatteryDrain();
    }
}