using System;
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface IGeoData
    {
        TollRoad GetTollRoad(long id);
        TollRoadWaypoint GetTollWayPoint(long id);
        TollPoint GetTollPoint(long id);

        IList<TollPoint> GetAllEntranceTollPoints();
        IList<TollPoint> GetAllExitTollPoints(long tollRoadId = -1);
    }
}
