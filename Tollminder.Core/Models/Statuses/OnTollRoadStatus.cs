using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{

		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
            Log.LogMessage(string.Format($"TRY TO FIND WAYPOINT EXIT FROM {SettingsService.WaypointLargeRadius * 1000} m"));

            var location = GeoWatcher.Location;
            var waypoint = DataService.FindNearestExitWaypoint(location, WaypointChecker.IgnoredChoiceTollPoint);

            if (waypoint == null)
            {
                Log.LogMessage($"No waypoint founded for location {GeoWatcher.Location}");
                WaypointChecker.SetCurrentTollPoint(null);

                return TollGeolocationStatus.OnTollRoad;
            }

            Log.LogMessage(string.Format("FOUNDED WAYPOINT : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));

            if (WaypointChecker.CurrentTollPoint?.Equals(waypoint) ?? false)
            {
                Log.LogMessage("Waypoint equals to currentWaypoint");
                return TollGeolocationStatus.OnTollRoad;
            }

            WaypointChecker.SetCurrentTollPoint(waypoint);
            GeoWatcher.StartUpdatingHighAccuracyLocation();

            return TollGeolocationStatus.NearTollRoadExit;
		}

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestWaypoint(GeoWatcher.Location, DataService.GetAllExitWaypoints(WaypointChecker.TollRoad.Id))?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
	}
}

