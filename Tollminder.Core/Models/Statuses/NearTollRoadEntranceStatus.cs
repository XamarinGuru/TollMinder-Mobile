using System;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class NearTollRoadEntranceStatus : BaseStatus
	{
		public override TollGeolocationStatus CheckStatus ()
		{
			Log.LogMessage (string.Format ("Is Closer to waypoint : {0}", DistanceChecker.IsCloserToWaypoint));

			if (DistanceChecker.IsCloserToWaypoint) {

				Log.LogMessage (string.Format ("DISTANCE CAR TO WAYPOINT IS CLOSER"));

				DistanceChecker.UpdateDistance ();
				if (DistanceChecker.IsAtWaypoint) {

					Log.LogMessage (string.Format ("CROSS WAYPOINT BY THE CAR"));
					Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));

					GeoWatcher.StopUpdatingHighAccuracyLocation ();
					NotifyService.Notify (string.Format ("You are entered to {0}", WaypointChecker.Waypoint.Name));
					return TollGeolocationStatus.OnTollRoad;
				}
				return TollGeolocationStatus.NearTollRoadEntrance;
			} else {

				Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));

				GeoWatcher.StopUpdatingHighAccuracyLocation ();
				return TollGeolocationStatus.NotOnTollRoad;
			}
		}

		public override void MakeActionForStatus()
		{
			SpeechToTextService.AskQuestion("Are you entering the tollroad?").Wait();
		}
	}
}

