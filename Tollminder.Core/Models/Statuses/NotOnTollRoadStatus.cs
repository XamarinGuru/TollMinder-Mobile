using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
	public class NotOnTollRoadStatus : BaseStatus 
	{
		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
			Log.LogMessage (string.Format ($"TRY TO FIND WAYPOINT ENTRANCE FROM {SettingsService.DistanceToWaypointRadius * 1000} m"));

			var location = GeoWatcher.Location;
            var waypoint = DataService.FindNearestWaypoint(location, WaypointAction.Enterce);

			if (waypoint == null)
			{
				Log.LogMessage($"No waypoint founded for location {GeoWatcher.Location}");
				WaypointChecker.SetCurrentWaypoint(null);

				return TollGeolocationStatus.NotOnTollRoad;
			}

			Log.LogMessage (string.Format ("FOUNDED WAYPOINT : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));
			
            if (WaypointChecker.CurrentWaypoint?.Equals(waypoint) ?? false)
			{
				Log.LogMessage("Waypoint equals to currentWaypoint");
				return TollGeolocationStatus.NotOnTollRoad;
			}

            WaypointChecker.SetCurrentWaypoint(waypoint);
            GeoWatcher.StartUpdatingHighAccuracyLocation();

            return TollGeolocationStatus.NearTollRoadEntrance;
		}

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestWaypoint(GeoWatcher.Location, DataService.GetAllWaypoints(WaypointAction.Enterce))?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
	}
}

