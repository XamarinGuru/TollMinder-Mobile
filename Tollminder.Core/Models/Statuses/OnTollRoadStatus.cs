using System.Linq;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{

		public override Task<TollGeolocationStatus> CheckStatus ()
		{
            Log.LogMessage(string.Format($"TRY TO FIND TOLLPOINT EXITS FROM {SettingsService.WaypointLargeRadius * 1000} m"));

            var location = GeoWatcher.Location;
            var waypoints = DataService.FindNearestExitTollPoints(location);

            WaypointChecker.SetTollPointsInRadius(waypoints);
            WaypointChecker.SetIgnoredChoiceTollPoint(null);

            if (waypoints.Count == 0)
            {
                GeoWatcher.StopUpdatingHighAccuracyLocation();
                Log.LogMessage($"No waypoint founded for location {GeoWatcher.Location}");
                return Task.FromResult(TollGeolocationStatus.OnTollRoad);
            }
            else
            {
                foreach (var item in WaypointChecker.TollPointsInRadius)
                    Log.LogMessage($"FOUNDED WAYPOINT : {item.Name}, DISTANCE {item.Distance}");

                GeoWatcher.StartUpdatingHighAccuracyLocation();

                return Task.FromResult(TollGeolocationStatus.NearTollRoadExit);
            }
		}

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestTollPoint(GeoWatcher.Location, DataService.GetAllExitTollPoints(WaypointChecker.TollRoad.Id)).FirstOrDefault()?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
	}
}

