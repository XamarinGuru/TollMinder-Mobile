using System;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class NearTollRoadStatus : BaseStatus
	{
		public override TollGeolocationStatus CheckStatus ()
		{
			#if DEBUG
			Log.LogMessage (string.Format ("Is Closer to waypoint : {0}", DistanceChecker.IsCloserToWaypoint));
			#endif
			if (DistanceChecker.IsCloserToWaypoint) {
				#if DEBUG
				Log.LogMessage (string.Format ("DISTANCE CAR TO WAYPOINT IS CLOSER"));
				#endif
				DistanceChecker.UpdateDistance ();
				if (DistanceChecker.IsAtWaypoint) {
					#if DEBUG
					Log.LogMessage (string.Format ("CROSS WAYPOINT BY THE CAR"));
					#endif
					#if DEBUG
					Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));
					#endif
					GeoWatcher.StopUpdatingHighAccuracyLocation ();
					NotifyService.Notify (string.Format ("You are entered to {0}", WaypointChecker.Waypoint.Name));
					return TollGeolocationStatus.OnTollRoad;
				}
				return TollGeolocationStatus.NearTollRoadEnterce;
			} else {
				#if DEBUG
				Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));
				#endif
				GeoWatcher.StopUpdatingHighAccuracyLocation ();
				return TollGeolocationStatus.NotOnTollRoad;
			}
		}
	}
}

