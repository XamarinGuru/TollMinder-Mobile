using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class NotOnTollRoadStatus : BaseStatus 
	{
		public override TollGeolocationStatus CheckStatus ()
		{
			Log.LogMessage (string.Format ("TRY TO FIND WAYPOINT ENTERCE FROM 200 m"));

			var location = GeoWatcher.Location;
			var waypoint = DataService.FindNearGeoLocation(location, WaypointAction.Enterce);

			Log.LogMessage (string.Format ("CAR LOCATION {0} , WAYPOINT LOCATION {1}", location, WaypointChecker.Waypoint));

			if (waypoint == null || waypoint == WaypointChecker.Waypoint)
				return TollGeolocationStatus.NotOnTollRoad;

			Log.LogMessage (string.Format ("FOUNDED WAYPOINT ENTERCE : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));

			WaypointChecker.Waypoint = waypoint;
			GeoWatcher.StartUpdatingHighAccuracyLocation ();
			NotifyService.Notify (string.Format ("you are potentially going to enter {0} waypoints.", WaypointChecker.Waypoint.Name));
			return TollGeolocationStatus.NearTollRoadEntrance;				
		}

		public override void MakeActionForStatus()
		{
			BatteryDrainService.CheckGpsTrackingSleepTime();
		}
		
	}
}

