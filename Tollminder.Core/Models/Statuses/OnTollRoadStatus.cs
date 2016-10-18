using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{

		public override Task<TollGeolocationStatus> CheckStatus ()
		{
            Log.LogMessage(string.Format($"TRY TO FIND WAYPOINT EXIT FROM {SettingsService.WaypointLargeRadius * 1000} m"));

            var location = GeoWatcher.Location;
            var waypoint = DataService.FindNearestWaypoint(location, WaypointAction.Exit, WaypointChecker.IgnoredChoiceWaypoint);

            if (waypoint == null)
            {
                Log.LogMessage($"No waypoint founded for location {GeoWatcher.Location}");
                WaypointChecker.SetCurrentWaypoint(null);

                return Task.FromResult(TollGeolocationStatus.OnTollRoad);
            }

            Log.LogMessage(string.Format("FOUNDED WAYPOINT : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));

            if (WaypointChecker.CurrentWaypoint?.Equals(waypoint) ?? false)
            {
                Log.LogMessage("Waypoint equals to currentWaypoint");
                return Task.FromResult(TollGeolocationStatus.OnTollRoad);
            }

            WaypointChecker.SetCurrentWaypoint(waypoint);
            GeoWatcher.StartUpdatingHighAccuracyLocation();

            return Task.FromResult(TollGeolocationStatus.NearTollRoadExit);
		}

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestWaypoint(GeoWatcher.Location, DataService.GetAllWaypoints(WaypointAction.Enterce, WaypointChecker.TollRoad?.Id ?? -1))?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
	}
}

