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
        TollRoadWaypoint IgnoredChoiceWaypoint { get; }
        double DistanceToNextWaypoint { get; }
		void SetCurrentWaypoint(TollRoadWaypoint point);
		void SetEntrance(TollRoadWaypoint point);
		void SetExit(TollRoadWaypoint point);
        void SetIgnoredChoiceWaypoint(TollRoadWaypoint point);

        void CreateBill();

        bool IsCloserToNextWaypoint(GeoLocation location);
        bool IsAtNextWaypoint(GeoLocation location);
        void UpdateDistanceToNextWaypoint(GeoLocation location);
	}
}

