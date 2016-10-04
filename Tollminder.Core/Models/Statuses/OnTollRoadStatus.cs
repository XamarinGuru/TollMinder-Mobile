using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{
        const double WaypointAreaDistanceRequired = 0.05;

		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
            var flag = WaypointChecker.DistanceToNextWaypoint - WaypointAreaDistanceRequired < double.Epsilon;
            WaypointChecker.UpdateDistanceToNextWaypoint(GeoWatcher.Location);
            Log.LogMessage($"Check if distance from current location to next waypoint is less than {WaypointAreaDistanceRequired*1000} m. [{flag}]");

            if (flag)
            {
                GeoWatcher.StartUpdatingHighAccuracyLocation();
                return TollGeolocationStatus.NearTollRoadExit;
            }
            else
                return TollGeolocationStatus.OnTollRoad;
		}

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestWaypoint(GeoWatcher.Location, new List<TollRoadWaypoint>() { WaypointChecker.NextWaypoint })?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
	}
}

