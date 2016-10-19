using System;
using Tollminder.Core.Models;
using System.Linq;
using System.Collections.Generic;

namespace Tollminder.Core.Services
{
    public interface IGeoDataService
    {
        TollRoad GetTollRoad(long id);
        TollRoadWaypoint GetTollWayPoint(long id);
        TollPoint GetTollPoint(long id);
        List<TollPointWithDistance> FindNearestEntranceTollPoints(GeoLocation center, TollPoint ignoredWaypoint = null);
        List<TollPointWithDistance> FindNearestExitTollPoints(GeoLocation center, TollPoint ignoredWaypoint = null);
        List<TollPoint> GetAllEntranceTollPoints();
        List<TollPoint> GetAllExitTollPoints(long tollRoadId = -1);
    }
}

