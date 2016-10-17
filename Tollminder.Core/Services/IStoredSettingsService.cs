using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IStoredSettingsService
	{
		bool GeoWatcherIsRunning { get; set; }
		GeoLocation Location { get; set; }
        TollRoad TollRoad { get; set; }
		TollGeolocationStatus Status { get; set; }
        TollPoint CurrentTollPoint { get; set; }
		TollRoadWaypoint TollRoadEntranceWaypoint { get; set; }
		TollRoadWaypoint TollRoadExitWaypoint { get; set; }
        TollPoint IgnoredChoiceTollPoint { get; set; }
        double DistanceToNextWaypoint { get; set; }
		DateTime SleepGPSDateTime { get; set; }
	}
}

