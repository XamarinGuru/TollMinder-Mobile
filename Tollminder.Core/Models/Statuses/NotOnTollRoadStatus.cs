using System.Threading.Tasks;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class NotOnTollRoadStatus : BaseStatus 
	{
		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
			Log.LogMessage (string.Format ("TRY TO FIND WAYPOINT ENTERCE FROM 200 m"));

			var location = GeoWatcher.Location;
			var waypoint = DataService.FindNearGeoLocation(location, WaypointAction.Enterce);

			Log.LogMessage (string.Format ("CAR LOCATION {0} , WAYPOINT LOCATION {1}", location, waypoint));

			if (waypoint == null)
			{
				BatteryDrainService.CheckGpsTrackingSleepTime();
				return TollGeolocationStatus.NotOnTollRoad;
			}
			Log.LogMessage (string.Format ("FOUNDED WAYPOINT ENTERCE : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));

			WaypointChecker.CurrentWaypoint = waypoint;
			NotifyService.Notify (string.Format ("You are potentially going to enter {0} waypoints.",waypoint.Name));
			if (await SpeechToTextService.AskQuestion($"Are you entering {WaypointChecker.CurrentWaypoint.Name} tollroad?"))
			{
				WaypointChecker.Entrance = WaypointChecker.CurrentWaypoint;
				return TollGeolocationStatus.OnTollRoad;
			}

			return TollGeolocationStatus.NotOnTollRoad;
		}
	}
}

