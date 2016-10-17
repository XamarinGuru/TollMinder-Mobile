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
        TollPoint FindNearestEntranceTollPoint(GeoLocation center, TollPoint ignoredWaypoint = null);
        TollPoint FindNearestExitTollPoint(GeoLocation center, TollPoint ignoredWaypoint = null);
        IList<TollPoint> GetAllEntranceTollPoints();
        IList<TollPoint> GetAllExitTollPoints(long tollRoadId = -1);
    }
}

