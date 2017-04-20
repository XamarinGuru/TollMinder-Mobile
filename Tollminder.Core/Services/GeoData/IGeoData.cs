using System;
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface IGeoData
    {
        TollRoad GetTollRoad(string id);
        TollRoadWaypoint GetTollWayPoint(string id);
        TollPoint GetTollPoint(string id);

        IList<TollPoint> GetAllTollPoints();
        IList<TollPoint> GetAllEntranceTollPoints();
        IList<TollPoint> GetAllExitTollPoints(string tollRoadId = "-1");
    }
}
