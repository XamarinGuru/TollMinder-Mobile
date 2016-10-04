using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IWaypointChecker
	{
		TollRoadWaypoint CurrentWaypoint { get; }
        TollRoad TollRoad { get; }
		TollRoadWaypoint Entrance { get; }
		TollRoadWaypoint Exit { get; }
        TollRoadWaypoint NextWaypoint { get; }
        double DistanceToNextWaypoint { get; }
		void SetCurrentWaypoint(TollRoadWaypoint point);
		void SetEntrance(TollRoadWaypoint point);
		void SetExit(TollRoadWaypoint point);
        void SetNextWaypoint(TollRoadWaypoint point);

        void CreateBill();

        bool IsCloserToNextWaypoint(GeoLocation location);
        bool IsAtNextWaypoint(GeoLocation location);
        void UpdateDistanceToNextWaypoint(GeoLocation location);
	}
}

