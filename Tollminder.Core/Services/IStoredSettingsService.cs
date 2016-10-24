using System;
using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IStoredSettingsService
	{
		bool GeoWatcherIsRunning { get; set; }
		GeoLocation Location { get; set; }
        TollRoad TollRoad { get; set; }
		TollGeolocationStatus Status { get; set; }
        List<TollPointWithDistance> TollPointsInRadius { get; set; }
		TollRoadWaypoint TollRoadEntranceWaypoint { get; set; }
        DateTime TollRoadEntranceWaypointDateTime { get; set; }
		TollRoadWaypoint TollRoadExitWaypoint { get; set; }
        DateTime TollRoadExitWaypointDateTime { get; set; }
        TollPoint IgnoredChoiceTollPoint { get; set; }
		DateTime SleepGPSDateTime { get; set; }
	}
}

