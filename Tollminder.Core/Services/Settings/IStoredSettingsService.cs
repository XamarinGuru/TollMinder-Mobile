﻿using System;
using System.Collections.Generic;
using Tollminder.Core.Models;
using Tollminder.Core.Models.PaymentData;

namespace Tollminder.Core.Services.Settings
{
    public interface IStoredSettingsService
    {
        bool GeoWatcherIsRunning { get; set; }
        GeoLocation Location { get; set; }
        TollRoad TollRoad { get; set; }
        TollPoint TollPoint { get; set; }
        TollGeolocationStatus Status { get; set; }
        TollGeolocationStatus CurrentRoadStatus { get; set; }
        List<TollPointWithDistance> TollPointsInRadius { get; set; }
        TollRoadWaypoint TollRoadEntranceWaypoint { get; set; }
        DateTime TollRoadEntranceWaypointDateTime { get; set; }
        TollRoadWaypoint TollRoadExitWaypoint { get; set; }
        DateTime TollRoadExitWaypointDateTime { get; set; }
        TollPoint IgnoredChoiceTollPoint { get; set; }
        DateTime SleepGPSDateTime { get; set; }
        decimal DistanceToNearestTollpoint { get; set; }

        TripCompleted TripCompleted { get; set; }

        bool IsAuthorized { get; set; }
        bool IsDataSynchronized { get; set; }
        string AuthToken { get; set; }
        string ProfileId { get; set; }
        Profile Profile { get; set; }

        DateTime LastSyncDateTime { get; set; }
    }
}

