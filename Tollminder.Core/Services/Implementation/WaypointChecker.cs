using System;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using System.Collections.Generic;

namespace Tollminder.Core.Services.Implementation
{
    public class WaypointChecker : IWaypointChecker
    {
        readonly IStoredSettingsService StoredSettingsService;
        readonly IGeoDataService GeoDataService;
        readonly IMvxMessenger Messenger;

        public List<TollPointWithDistance> TollPointsInRadius
        {
            get
            {
                return StoredSettingsService.TollPointsInRadius;
            }
            private set
            {
                StoredSettingsService.TollPointsInRadius = value;
                Messenger.Publish(new CurrentTollpointChangedMessage(this, value));
            }
        }

        public TollRoadWaypoint Entrance
        {
            get
            {
                return StoredSettingsService.TollRoadEntranceWaypoint;
            }
            private set
            {
                StoredSettingsService.TollRoadEntranceWaypoint = value;
                StoredSettingsService.TollRoadEntranceWaypointDateTime = (value == null) ? DateTime.MinValue : DateTime.Now;
            }
        }

        public TollRoadWaypoint Exit
        {
            get
            {
                return StoredSettingsService.TollRoadExitWaypoint;
            }
            private set
            {
                StoredSettingsService.TollRoadExitWaypoint = value;
                StoredSettingsService.TollRoadExitWaypointDateTime = (value == null) ? DateTime.MinValue : DateTime.Now;
            }
        }

        public TollPoint IgnoredChoiceTollPoint
        {
            get
            {
                return StoredSettingsService.IgnoredChoiceTollPoint;
            }
            private set
            {
                StoredSettingsService.IgnoredChoiceTollPoint = value;
            }
        }

        public decimal DistanceToNearestTollpoint
        {
            get
            {
                return StoredSettingsService.DistanceToNearestTollpoint;
            }
            private set
            {
                StoredSettingsService.DistanceToNearestTollpoint = value;
            }
        }

        public TollRoad TollRoad
        {
            get
            {
                return StoredSettingsService.TollRoad;
            }
            private set
            {
                StoredSettingsService.TollRoad = value;
                Messenger.Publish(new TollRoadChangedMessage(this, value));
            }
        }

        public TimeSpan TripDuration
        {
            get
            {
                if (Exit == null || Entrance == null)
                    throw new Exception("Trip not finished");

                return StoredSettingsService.TollRoadExitWaypointDateTime.Subtract(StoredSettingsService.TollRoadEntranceWaypointDateTime);
            }
        }

        public WaypointChecker()
        {
            StoredSettingsService = Mvx.Resolve<IStoredSettingsService>();
            GeoDataService = Mvx.Resolve<IGeoDataService>();
            Messenger = Mvx.Resolve<IMvxMessenger>();
        }

        public WaypointChecker(IStoredSettingsService storedSettingsService)
        {
            StoredSettingsService = storedSettingsService;
        }

        public void SetEntrance(TollPoint point)
        {
            Entrance = GeoDataService.GetTollWayPoint(point.TollWaypointId);
            TollRoad = GeoDataService.GetTollRoad(Entrance.TollRoadId);
        }

        public void SetExit(TollPoint point)
        {
            Exit = GeoDataService.GetTollWayPoint(point.TollWaypointId);
        }

        public void SetTollPointsInRadius(List<TollPointWithDistance> points)
        {
            if (points == null)
                points = new List<TollPointWithDistance>();

            TollPointsInRadius = points;
        }

        public void SetIgnoredChoiceTollPoint(TollPoint point)
        {
            IgnoredChoiceTollPoint = point;
        }

        public double UpdateDistanceToNextWaypoint(GeoLocation location, TollPoint point)
        {
            return LocationChecker.DistanceBetweenGeoLocations(location, point.Location);
        }

        public void ClearData()
        {
            TollRoad = null;
            Entrance = null;
            Exit = null;
            TollPointsInRadius.Clear();
            IgnoredChoiceTollPoint = null;
        }

        public TollPoint DetectWeAreInsideSomeTollPoint(GeoLocation location)
        {
            int nearestToolpoint = 0;
            if (TollPointsInRadius.Count != nearestToolpoint)
            {
                foreach (var item in TollPointsInRadius)
                {
                    if (item.Equals(IgnoredChoiceTollPoint))
                        break;

                    var distance = UpdateDistanceToNextWaypoint(location, item);
                    if (nearestToolpoint < 1)
                    {
                        nearestToolpoint++;
                        DistanceToNearestTollpoint = Math.Truncate(decimal.Parse((distance / 1.609344).ToString()) * 1000m) / 1000m;
                        Log.LogMessage($"Distance to nearest Tollpoint: {DistanceToNearestTollpoint}");
                    }
                    Log.LogMessage($"Distance to {item.Name} waypoint is {distance}");
                    if (distance - SettingsService.WaypointSmallRadius < double.Epsilon)
                    {
                        Log.LogMessage($"We are inside in Tollpoint: {item.Name}, Latitude: {item.Latitude}, Longitude: {item.Longitude}");
                        return item;
                    }
                }
            }
            else
                DistanceToNearestTollpoint = 0;

            return null;
        }
    }
}

