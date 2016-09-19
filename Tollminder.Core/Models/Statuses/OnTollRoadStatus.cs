using System;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{
		public override TollGeolocationStatus CheckStatus ()
		{
			Log.LogMessage (string.Format ("TRY TO FIND WAYPOINT EXIT FROM 200 m"));

			var waypoint = DataService.FindNearGeoLocation (GeoWatcher.Location, WaypointAction.Exit);

			if (waypoint == null || waypoint == WaypointChecker.Waypoint)
				return TollGeolocationStatus.OnTollRoad;

			Log.LogMessage (string.Format ("FOUNDED WAYPOINT EXIT : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));

			WaypointChecker.Waypoint = waypoint;
			GeoWatcher.StartUpdatingHighAccuracyLocation ();
			NotifyService.Notify (string.Format ("you are potentially going to exit {0} waypoints.", WaypointChecker.Waypoint.Name));
			return TollGeolocationStatus.NearTollRoadExit;
		}

		public override void MakeActionForStatus()
		{
			
		}
	}
}

