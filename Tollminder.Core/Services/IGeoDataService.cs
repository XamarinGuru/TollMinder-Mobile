using System;
using Tollminder.Core.Models;
using System.Linq;
using System.Collections.Generic;

namespace Tollminder.Core.Services
{
    public interface IGeoDataService
    {
        TollRoad GetTollRoad(long id);
        TollRoadWaypoint FindNearestEntranceWaypoint(GeoLocation center, TollRoadWaypoint ignoredWaypoint = null);
        TollRoadWaypoint FindNearestExitWaypoint(GeoLocation center, TollRoadWaypoint ignoredWaypoint = null);
        IList<TollRoadWaypoint> GetAllEntranceWaypoints();
        IList<TollRoadWaypoint> GetAllExitWaypoints(long tollRoadId = -1);
    }
}

