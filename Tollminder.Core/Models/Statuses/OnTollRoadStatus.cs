using System;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{
		public override TollGeolocationStatus CheckStatus ()
		{
			#if DEBUG
			Log.LogMessage (string.Format ("CHECKING IS MOVING ON THE CAR {0}", MotionActivity.IsAutomove));
			#endif
			if (MotionActivity.IsAutomove) {
				#if DEBUG
				Log.LogMessage (string.Format ("TRY TO FIND WAYPOINT EXIT FROM 200 m"));
				#endif
				var waypoint = DataService.FindNearGeoLocation (GeoWatcher.Location, WaypointAction.Exit);

				if (waypoint == null || waypoint == WaypointChecker.Waypoint)
					return TollGeolocationStatus.OnTollRoad;
				#if DEBUG
				Log.LogMessage (string.Format ("FOUNDED WAYPOINT EXIT : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));
				#endif
				WaypointChecker.Waypoint = waypoint;
				GeoWatcher.StartUpdatingHighAccuracyLocation ();
				return TollGeolocationStatus.NearTollRoadExit;

				//#if DEBUG
				//Log.LogMessage (string.Format ("ENABLED HIGH ACCURACY"));
				//#endif
				//NotifyUser (string.Format ("you are potentially going to exit {0} waypoints.", LastTollRoadWaypoint.Name));
			}
			return TollGeolocationStatus.OnTollRoad;
		}
	}
}

