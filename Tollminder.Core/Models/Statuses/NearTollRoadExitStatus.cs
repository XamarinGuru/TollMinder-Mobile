using System;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class NearTollRoadExitStatus : BaseStatus
	{
		public override TollGeolocationStatus CheckStatus ()
		{
			Log.LogMessage (string.Format ("Is Closer to waypoint : {0}", DistanceChecker.IsCloserToWaypoint));

			if (DistanceChecker.IsCloserToWaypoint) 
			{
				Log.LogMessage (string.Format ("DISTANCE CAR TO WAYPOINT IS CLOSER"));

				DistanceChecker.UpdateDistance ();
				if (DistanceChecker.IsAtWaypoint) {

					Log.LogMessage (string.Format ("CROSS WAYPOINT BY THE CAR"));
					Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));

					GeoWatcher.StopUpdatingHighAccuracyLocation ();
					NotifyService.Notify (string.Format ("You are entered to {0}", WaypointChecker.Waypoint.Name));
					return TollGeolocationStatus.NotOnTollRoad;
				}
				return TollGeolocationStatus.NearTollRoadExit;
			} else {

				Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));

				GeoWatcher.StopUpdatingHighAccuracyLocation ();
				return TollGeolocationStatus.OnTollRoad;
			}
		}

		public override void MakeActionForStatus()
		{
			SpeechToTextService.AskQuestion("Are you exiting from the tollroad?");
		}
	}
}

