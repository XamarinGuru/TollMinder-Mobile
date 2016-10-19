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
		readonly IStoredSettingsService _storedSettingsService;

        IGeoDataService _geoDataService;
        IGeoDataService GeoDataService
        {
            get
            {
                return _geoDataService ?? (_geoDataService = Mvx.Resolve<IGeoDataService>());
            }
        }

        IMvxMessenger _messenger;
        IMvxMessenger Messeger
        {
            get
            {
                return _messenger ?? (_messenger = Mvx.Resolve<IMvxMessenger>());
            }
        }

        public List<TollPointWithDistance> TollPointsInRadius 
		{
			get 
			{
				return _storedSettingsService.TollPointsInRadius; 
			}
			private set 
			{
				_storedSettingsService.TollPointsInRadius = value;
                Messeger.Publish(new CurrentWaypointChangedMessage(this, value));
			}
		}
	
		public TollRoadWaypoint Entrance
		{
			get
			{
				return _storedSettingsService.TollRoadEntranceWaypoint;
			}
			private set
			{
				_storedSettingsService.TollRoadEntranceWaypoint = value;
			}
		}

		public TollRoadWaypoint Exit
		{
			get
			{
				return _storedSettingsService.TollRoadExitWaypoint;
			}
			private set
			{
				_storedSettingsService.TollRoadExitWaypoint = value;
			}
		}

        public TollPoint IgnoredChoiceTollPoint
        {
            get
            {
                return _storedSettingsService.IgnoredChoiceTollPoint;
            }
            private set
            {
                _storedSettingsService.IgnoredChoiceTollPoint = value;
            }
        }

        public TollRoad TollRoad
        {
            get
            {
                return _storedSettingsService.TollRoad;
            }
            private set
            {
                _storedSettingsService.TollRoad = value;
                Messeger.Publish(new TollRoadChangedMessage(this, value));
            }
        }

        public WaypointChecker(IStoredSettingsService storedSettingsService)
		{
			_storedSettingsService = storedSettingsService;
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

        public void CreateBill()
        {
            TollRoad = null;
            Entrance = null;
            Exit = null;
            TollPointsInRadius.Clear();
            IgnoredChoiceTollPoint = null;
        }

        public TollPoint DetectWeAreInsideSomeTollPoint(GeoLocation location)
        {
            foreach(var item in TollPointsInRadius)
            {
                var distance = UpdateDistanceToNextWaypoint(location, item);
                Log.LogMessage($"Distance to {item.Name} waypoint is {distance}");
                if (distance - SettingsService.WaypointSmallRadius < double.Epsilon)
                    return item;
            }

            return null;
        }
    }
}

