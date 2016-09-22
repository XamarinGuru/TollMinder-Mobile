using System;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{
		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
			Log.LogMessage(string.Format($"TRY TO FIND WAYPOINT ENTERCE FROM {Services.Implementation.DistanceChecker.DistanceToWaypoint * 10} m"));

			var waypoint = DataService.FindNearGeoLocation (GeoWatcher.Location, WaypointAction.Exit);

			if (waypoint == null)
			{
				WaypointChecker.SetCurrentWaypoint(null);
				return TollGeolocationStatus.OnTollRoad;
			}

			Log.LogMessage (string.Format ("FOUNDED WAYPOINT EXIT : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));
			if (WaypointChecker.CurrentWaypoint?.Equals(waypoint) ?? false)
			{
				return TollGeolocationStatus.OnTollRoad; 
			}

			WaypointChecker.SetCurrentWaypoint(waypoint);

			NotifyService.Notify (string.Format ("you are potentially going to exit {0} waypoints.", waypoint.Name));
			if (await SpeechToTextService.AskQuestion($"Are you exiting {WaypointChecker.CurrentWaypoint.Name} the tollroad?"))
			{
				WaypointChecker.SetExit(WaypointChecker.CurrentWaypoint);
				return TollGeolocationStatus.NotOnTollRoad;
			}
			return TollGeolocationStatus.OnTollRoad;
		}
	}
}

