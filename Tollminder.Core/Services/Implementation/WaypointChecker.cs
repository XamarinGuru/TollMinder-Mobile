using MvvmCross.Platform;
using Tollminder.Core.Models;

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

        public TollRoad TollRoad
        {
            get
            {
                return _storedSettingsService.TollRoad;
            }
            private set
            {
                _storedSettingsService.TollRoad = value;
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
	}
}

