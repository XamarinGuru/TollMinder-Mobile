using System;
using Tollminder.Core.Models;
using System.Linq;
using System.Collections.Generic;

namespace Tollminder.Core.Services
{
    public interface IGeoDataService
    {
        TollRoad GetTollRoad(long id);
        TollRoadWaypoint FindNearestWaypoint(GeoLocation center, WaypointAction action);
        TollRoadWaypoint FindNextExitWaypoint(GeoLocation center, TollRoadWaypoint currentWaypoint);
        IList<TollRoadWaypoint> GetAllWaypoints(WaypointAction action, long tollRoadId = -1);
    }
}

