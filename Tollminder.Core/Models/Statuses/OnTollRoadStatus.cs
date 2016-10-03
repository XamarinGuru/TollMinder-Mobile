using System;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{
		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
			Log.LogMessage(string.Format($"TRY TO FIND WAYPOINT EXIT FROM {Services.Implementation.DistanceChecker.DistanceToWaypointRadius * 1000} m"));

            var waypoint = DataService.FindNearestTollRoad (GeoWatcher.Location, WaypointAction.Exit);

			if (waypoint == null)
			{
				Log.LogMessage($"No waypoint founded for location {GeoWatcher.Location}");
				WaypointChecker.SetCurrentWaypoint(null);

				return TollGeolocationStatus.OnTollRoad;
			}

			Log.LogMessage (string.Format ("FOUNDED WAYPOINT EXIT : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));
			if (WaypointChecker.CurrentWaypoint?.Equals(waypoint) ?? false)
			{
				Log.LogMessage("Waypoint equals to currentWaypoint");
				return TollGeolocationStatus.OnTollRoad; 
			}

			WaypointChecker.SetCurrentWaypoint(waypoint);

			await NotifyService.Notify (string.Format ("you are potentially going to exit {0} waypoints.", waypoint.Name));
			if (await SpeechToTextService.AskQuestion($"Are you exiting {WaypointChecker.CurrentWaypoint.Name} the tollroad?"))
			{
				WaypointChecker.SetExit(WaypointChecker.CurrentWaypoint);
				return TollGeolocationStatus.NotOnTollRoad;
			}
            else
            {
                WaypointChecker.SetNextWaypoint(DataService.FindNextTollRoad(WaypointChecker.NextWaypoint));
            }
			return TollGeolocationStatus.OnTollRoad;
		}
	}
}

