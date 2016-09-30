using System;
using Tollminder.Core.Models;
using System.Linq;
using System.Collections.Generic;

namespace Tollminder.Core.Services
{
    public interface IGeoDataService
    {
        TollRoad GetTollRoad(long id);
        TollRoadWaypoint FindNearestTollRoad(GeoLocation center, WaypointAction action);
        IList<TollRoadWaypoint> GetAllWaypoints(WaypointAction action);
    }
}

