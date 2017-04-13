using System;

namespace Tollminder.Core.Models
{
    public enum TollGeolocationStatus
    {
        NearestTollPoint,
        NotOnTollRoad,
        OnTollRoad,
        NearTollRoadExit,
        NearTollRoadEntrance
    }
}

