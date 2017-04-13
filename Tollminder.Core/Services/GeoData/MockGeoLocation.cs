using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Settings;
using System.Threading.Tasks;
using System.Collections.Generic;
using MvvmCross.Binding.ExtensionMethods;
using Tollminder.Core.Services.RoadsProcessing;
using System;

namespace Tollminder.Core.Services.GeoData
{
    public class MockGeoLocation : IGeoLocationWatcher, IMotionActivity, IMockGeoLocation
    {
        readonly IStoredSettingsService _storedSettingsService;
        readonly IMvxMessenger _messenger;
        readonly IDataBaseService dataBaseService;
        private IList<TollRoad> getRoads;
        private int roadId = 6;
        private bool isLocationStarted;
        private WaypointAction waypointAction;

        public MockGeoLocation(IStoredSettingsService storedSettingsService, IMvxMessenger messenger, IDataBaseService dataBaseService)
        {
            this._storedSettingsService = storedSettingsService;
            this._messenger = messenger;
            this.dataBaseService = dataBaseService;
        }

        public bool IsBound
        {
            get
            {
                return _storedSettingsService.GeoWatcherIsRunning;
            }
            set
            {
                _storedSettingsService.GeoWatcherIsRunning = value;
                _messenger.Publish(new GeoWatcherStatusMessage(this, value));
            }
        }

        private GeoLocation location;
        public GeoLocation Location
        {
            get { return location; }
            set
            {
                Log.LogMessage($"Received location in geowatcher {value}");
                if (IsBound && (!location?.Equals(value) ?? true))
                {
                    location = value;
                    Mvx.Resolve<IMvxMessenger>().Publish(new LocationMessage(this, value));
                    Log.LogMessage($"New location {value}");
                }
            }
        }

        private MotionType _motionType;
        public MotionType MotionType
        {
            get { return _motionType; }
            set
            {
                _motionType = value;
                Mvx.Resolve<IMvxMessenger>().Publish(new MotionMessage(this, value));
            }
        }

        private async void StartLocationUpdates()
        {
            getRoads = getRoads != null ? getRoads : await dataBaseService.GetTollRoadList();
            if (getRoads.Count != 0)
            {
                IsBound = true;
                isLocationStarted = true;
                var getTollRoad = (TollRoad)getRoads.ElementAt(roadId);
                var tollPoint = getTollRoad.WayPoints.Find(wayPoint => wayPoint.TollPoints.Find(point => point.WaypointAction == waypointAction));
                Location = new GeoLocation()
                {
                    Latitude = tollPoint.Latitude,
                    Longitude = tollPoint.Longitude,
                    Tol
                };
                foreach (var road in getTollRoad.WayPoints)
                {
                    if (isLocationStarted)
                    {
                        foreach (var point in road.TollPoints)
                        {
                            if (isLocationStarted)
                            {
                                if (point.WaypointAction == waypointAction)
                                {
                                    Location = new GeoLocation()
                                    {
                                        Latitude = point.Latitude,
                                        Longitude = point.Longitude,
                                        TollPointId = point.Id
                                    };
                                    return;
                                }
                                //await Task.Delay(10000);
                            }
                        }
                        //return;
                        //await Task.Delay(10000);
                    }
                }
            }
        }

        private void StopLocationUpdates()
        {
            isLocationStarted = false;
        }

        public virtual void StartGeolocationWatcher()
        {
            if (!IsBound)
            {
                StartLocationUpdates();
                IsBound = true;
            }
        }

        public virtual void StopGeolocationWatcher()
        {
            if (IsBound)
            {
                StopLocationUpdates();
                IsBound = false;
            }
        }

        public virtual void StartUpdatingHighAccuracyLocation()
        {
            UpdateAccuracyLocation(SettingsService.DistanceIntervalHighDefault);
        }

        public virtual void StopUpdatingHighAccuracyLocation()
        {
            UpdateAccuracyLocation(SettingsService.DistanceIntervalDefault);
        }

        void UpdateAccuracyLocation(int distanceInterval)
        {
            if (IsBound)
            {
                StopLocationUpdates();
                StartLocationUpdates();
            }
        }

        public void StartDetection()
        {
            if (!IsBound)
            {
                MotionType = MotionType.Automotive;
            }
        }

        public void StopDetection()
        {
            if (IsBound)
                IsBound = false;
        }

        public void NextTollPoint(WaypointAction waypointAction)
        {
            this.waypointAction = waypointAction;
            StartLocationUpdates();
        }
    }
}
