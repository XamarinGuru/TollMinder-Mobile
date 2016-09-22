using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IWaypointChecker
	{
		TollRoadWaypoint CurrentWaypoint { get; }
		TollRoadWaypoint Entrance { get; }
		TollRoadWaypoint Exit { get; }
		void SetCurrentWaypoint(TollRoadWaypoint point);
		void SetEntrance(TollRoadWaypoint point);
		void SetExit(TollRoadWaypoint point);
	}
}

