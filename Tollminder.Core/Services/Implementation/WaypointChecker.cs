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
        readonly IStoredSettingsService storedSettingsService;
        readonly IGeoDataService geoDataService;
        readonly IMvxMessenger messenger;

        public List<TollPointWithDistance> TollPointsInRadius
        {
            get
            {
                return storedSettingsService.TollPointsInRadius;
            }
            private set
            {
                storedSettingsService.TollPointsInRadius = value;
                messenger.Publish(new CurrentTollpointChangedMessage(this, value));
            }
        }

        public TollRoadWaypoint Entrance
        {
            get
            {
                return storedSettingsService.TollRoadEntranceWaypoint;
            }
            private set
            {
                storedSettingsService.TollRoadEntranceWaypoint = value;
                storedSettingsService.TollRoadEntranceWaypointDateTime = (value == null) ? DateTime.MinValue : DateTime.Now;
            }
        }

        public TollRoadWaypoint Exit
        {
            get
            {
                return storedSettingsService.TollRoadExitWaypoint;
            }
            private set
            {
                storedSettingsService.TollRoadExitWaypoint = value;
                storedSettingsService.TollRoadExitWaypointDateTime = (value == null) ? DateTime.MinValue : DateTime.Now;
            }
        }

        public TollPoint IgnoredChoiceTollPoint
        {
            get
            {
                return storedSettingsService.IgnoredChoiceTollPoint;
            }
            private set
            {
                storedSettingsService.IgnoredChoiceTollPoint = value;
            }
        }

        public decimal DistanceToNearestTollpoint
        {
            get
            {
                return storedSettingsService.DistanceToNearestTollpoint;
            }
            private set
            {
                storedSettingsService.DistanceToNearestTollpoint = value;
            }
        }

        public TollRoad TollRoad
        {
            get
            {
                return storedSettingsService.TollRoad;
            }
            private set
            {
                storedSettingsService.TollRoad = value;
                messenger.Publish(new TollRoadChangedMessage(this, value));
            }
        }

        public TollPoint TollPoint
        {
            get { return storedSettingsService.TollPoint; }
            private set { storedSettingsService.TollPoint = value; }
        }

        public TimeSpan TripDuration
        {
            get
            {
                if (Exit == null || Entrance == null)
                    throw new Exception("Trip not finished");

                return storedSettingsService.TollRoadExitWaypointDateTime.Subtract(storedSettingsService.TollRoadEntranceWaypointDateTime);
            }
        }

        public WaypointChecker()
        {
            storedSettingsService = Mvx.Resolve<IStoredSettingsService>();
            geoDataService = Mvx.Resolve<IGeoDataService>();
            messenger = Mvx.Resolve<IMvxMessenger>();
        }

        public WaypointChecker(IStoredSettingsService storedSettingsService)
        {
            storedSettingsService = storedSettingsService;
        }

        public void SetEntrance(TollPoint point)
        {
            Entrance = geoDataService.GetTollWayPoint(point.TollWaypointId);
            TollRoad = geoDataService.GetTollRoad(Entrance.TollRoadId);
            TollPoint = geoDataService.GetTollPoint(point.Id);
        }

        public void SetExit(TollPoint point)
        {
            Exit = geoDataService.GetTollWayPoint(point.TollWaypointId);
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
                    double radius = item.Radius != 0 ? item.Radius / 1000 : SettingsService.WaypointSmallRadius;
                    var distance = UpdateDistanceToNextWaypoint(location, item);
                    if (nearestToolpoint < 1)
                    {
                        nearestToolpoint++;
                        DistanceToNearestTollpoint = Math.Truncate(decimal.Parse((distance / 1.609344).ToString()) * 1000m) / 1000m;
                        Log.LogMessage($"Distance to nearest Tollpoint: {DistanceToNearestTollpoint}");
                    }
                    Log.LogMessage($"Distance to {item.Name} waypoint is {distance}");
                    if (distance - (radius) < double.Epsilon)
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

