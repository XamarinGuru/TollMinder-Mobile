using System;
using MvvmCross.Platform;
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
			set 
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
			set
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
			set
			{
				_exit = value;
			}
		}
	}
}

