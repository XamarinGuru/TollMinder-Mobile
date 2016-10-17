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
        TollPoint FindNearestEntranceWaypoint(GeoLocation center, TollPoint ignoredWaypoint = null);
        TollPoint FindNearestExitWaypoint(GeoLocation center, TollPoint ignoredWaypoint = null);
        IList<TollPoint> GetAllEntranceWaypoints();
        IList<TollPoint> GetAllExitWaypoints(long tollRoadId = -1);
    }
}

