using System;
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IWaypointChecker
	{
        List<TollPointWithDistance> TollPointsInRadius { get; }
        TollRoad TollRoad { get; }
		TollRoadWaypoint Entrance { get; }
		TollRoadWaypoint Exit { get; }
        TollPoint IgnoredChoiceTollPoint { get; }
        void SetTollPointsInRadius(List<TollPointWithDistance> points);
        void SetEntrance(TollPoint point);
        void SetExit(TollPoint point);
        void SetIgnoredChoiceTollPoint(TollPoint point);

        void CreateBill();

        TollPoint DetectWeAreInsideSomeTollPoint(GeoLocation location);
	}
}

