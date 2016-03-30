using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class NotOnTollRoadStatus : BaseStatus 
	{
		#region IStatus implementation
		public override TollGeolocationStatus CheckStatus ()
		{
			var asd = DistanceChecker.GetMostClosestWaypoint (GeoWatcher.Location, Services.Implementation.DummyDataSerivce._dummyWaypoints); 
			#if DEBUG 
			Log.LogMessage (string.Format ("TRY TO FIND WAYPOINT ENTERCE FROM 200 m"));
			#endif
			var waypoint = DataService.FindNearGeoLocation(GeoWatcher.Location, WaypointAction.Enterce);
			#if DEBUG 
			Log.LogMessage (string.Format ("CAR LOCATION {0} , WAYPOINT LOCATION {1}", GeoWatcher.Location, WaypointChecker.Waypoint));
			#endif
			if (waypoint == null || waypoint == WaypointChecker.Waypoint)
				return TollGeolocationStatus.NotOnTollRoad;
			#if DEBUG 
			Log.LogMessage (string.Format ("FOUNDED WAYPOINT ENTERCE : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));
			#endif
			WaypointChecker.Waypoint = waypoint;
			GeoWatcher.StartUpdatingHighAccuracyLocation ();
			NotifyService.Notify (string.Format ("you are potentially going to enter {0} waypoints.", WaypointChecker.Waypoint.Name));
			return TollGeolocationStatus.NearTollRoadEnterce;				
		}
		#endregion
		
	}
}

