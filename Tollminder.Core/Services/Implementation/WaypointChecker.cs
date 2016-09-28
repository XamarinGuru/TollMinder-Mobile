using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
	public class WaypointChecker : IWaypointChecker
	{
		readonly IStoredSettingsService _storedSettingsService;

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
		}

		public void SetExit(TollRoadWaypoint point)
		{
			Exit = point;
		}
	}
}

