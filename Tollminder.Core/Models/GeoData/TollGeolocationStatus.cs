using System;

namespace Tollminder.Core.Models
{
    public enum TollGeolocationStatus
    {
        SearchingNearestTollPoint,
        NotOnTollRoad,
        OnTollRoad,
        NearTollRoadExit,
        NearTollRoadEntrance
    }
}

