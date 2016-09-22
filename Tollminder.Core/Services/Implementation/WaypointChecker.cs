using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
	public class WaypointChecker : IWaypointChecker
	{
		TollRoadWaypoint _currentWaypoint;
		public TollRoadWaypoint CurrentWaypoint 
		{
			get 
			{
				return _currentWaypoint; 
			}
			private set 
			{
				_currentWaypoint = value;
			}
		}
	
		TollRoadWaypoint _entrance;
		public TollRoadWaypoint Entrance
		{
			get
			{
				return _entrance;
			}
			private set
			{
				_entrance = value;
			}
		}

		TollRoadWaypoint _exit;
		public TollRoadWaypoint Exit
		{
			get
			{
				return _exit;
			}
			private set
			{
				_exit = value;
			}
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

