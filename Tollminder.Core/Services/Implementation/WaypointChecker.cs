using System;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;

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

        public TollPoint CurrentTollPoint 
		{
			get 
			{
				return _storedSettingsService.CurrentTollPoint; 
			}
			private set 
			{
				_storedSettingsService.CurrentTollPoint = value;
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

        public double DistanceToNextWaypoint
        {
            get
            {
                return _storedSettingsService.DistanceToNextWaypoint;
            }
            set
            {
                _storedSettingsService.DistanceToNextWaypoint = value;
            }
        }

        public WaypointChecker(IStoredSettingsService storedSettingsService)
		{
			_storedSettingsService = storedSettingsService;
		}

        public void SetCurrentTollPoint(TollPoint point)
		{
			CurrentTollPoint = point;
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

        public void SetIgnoredChoiceTollPoint(TollPoint point)
        {
            IgnoredChoiceTollPoint = point;
            DistanceToNextWaypoint = Double.MaxValue;
        }

        public bool IsCloserToNextWaypoint(GeoLocation location)
        {
            var oldDistance = DistanceToNextWaypoint;
            UpdateDistanceToNextWaypoint(location);
            return DistanceToNextWaypoint - oldDistance < double.Epsilon;
        }

        public bool IsAtNextWaypoint(GeoLocation location)
        {
            UpdateDistanceToNextWaypoint(location);
            return DistanceToNextWaypoint - SettingsService.WaypointSmallRadius < double.Epsilon;
        }

        public void UpdateDistanceToNextWaypoint(GeoLocation location)
        {
            DistanceToNextWaypoint = LocationChecker.DistanceBetweenGeoLocations(location, CurrentTollPoint.Location);
        }

        public void CreateBill()
        {
            TollRoad = null;
            Entrance = null;
            Exit = null;
            IgnoredChoiceTollPoint = null;
            CurrentTollPoint = null;
        }
    }
}

