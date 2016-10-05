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
        const double WaypointDistanceRequired = 0.025;

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

		public TollRoadWaypoint CurrentWaypoint 
		{
			get 
			{
				return _storedSettingsService.CurrentWaypoint; 
			}
			private set 
			{
				_storedSettingsService.CurrentWaypoint = value;
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

        public TollRoadWaypoint NextWaypoint
        {
            get
            {
                return _storedSettingsService.NextWaypoint;
            }
            private set
            {
                _storedSettingsService.NextWaypoint = value;
                Messeger.Publish(new NextWaypointChangedMessage(this, value));
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

		public void SetCurrentWaypoint(TollRoadWaypoint point)
		{
			CurrentWaypoint = point;
		}

		public void SetEntrance(TollRoadWaypoint point)
		{
			Entrance = point;
            TollRoad = GeoDataService.GetTollRoad(point.TollRoadId);
		}

		public void SetExit(TollRoadWaypoint point)
		{
			Exit = point;
		}

        public void SetNextWaypoint(TollRoadWaypoint point)
        {
            NextWaypoint = point;
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
            return DistanceToNextWaypoint - WaypointDistanceRequired < double.Epsilon;
        }

        public void UpdateDistanceToNextWaypoint(GeoLocation location)
        {
            DistanceToNextWaypoint = LocationChecker.DistanceBetweenGeoLocations(location, NextWaypoint.Location);
        }

        public void CreateBill()
        {
            TollRoad = null;
            Entrance = null;
            Exit = null;
            NextWaypoint = null;
            CurrentWaypoint = null;
        }
    }
}

