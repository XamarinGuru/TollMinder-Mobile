using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IWaypointChecker
	{
		TollPoint CurrentTollPoint { get; }
        TollRoad TollRoad { get; }
		TollRoadWaypoint Entrance { get; }
		TollRoadWaypoint Exit { get; }
        TollPoint IgnoredChoiceTollPoint { get; }
        double DistanceToNextWaypoint { get; }
        void SetCurrentTollPoint(TollPoint point);
        void SetEntrance(TollPoint point);
        void SetExit(TollPoint point);
        void SetIgnoredChoiceTollPoint(TollPoint point);

        void CreateBill();

        bool IsCloserToNextWaypoint(GeoLocation location);
        bool IsAtNextWaypoint(GeoLocation location);
        void UpdateDistanceToNextWaypoint(GeoLocation location);
	}
}

