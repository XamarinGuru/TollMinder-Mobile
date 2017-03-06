using System;
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface IWaypointChecker
    {
        TollRoad TollRoad { get; }
        TollPoint TollPoint { get; }
        TollRoadWaypoint Entrance { get; }
        TollRoadWaypoint Exit { get; }
        TollPoint IgnoredChoiceTollPoint { get; }
        decimal DistanceToNearestTollpoint { get; }
        TimeSpan TripDuration { get; }
        List<TollPointWithDistance> TollPointsInRadius { get; }

        void SetTollPointsInRadius(List<TollPointWithDistance> points);
        void SetEntrance(TollPoint point);
        void SetExit(TollPoint point);
        void SetIgnoredChoiceTollPoint(TollPoint point);

        TollPoint DetectWeAreInsideSomeTollPoint(GeoLocation location);

        void ClearData();
    }
}

