using System.Linq;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Settings;
using Tollminder.Core.Models.GeoData;

namespace Tollminder.Core.Models.Statuses
{
    public class OnTollRoadStatus : BaseStatus
    {
        public override Task<TollGeoStatusResult> CheckStatus(TollGeolocationStatus tollGeoStatus)
        {
            Log.LogMessage(string.Format($"TRY TO FIND TOLLPOINT EXITS FROM {SettingsService.WaypointLargeRadius * 1000} m"));

            var location = GeoWatcher.Location;
            var nearestWaypoints = GeoDataService.FindNearestExitTollPoints(location);

            WaypointChecker.SetTollPointsInRadius(nearestWaypoints);

            WaypointChecker.SetIgnoredChoiceTollPoint(null);

            if (nearestWaypoints.Count == 0)
            {
                GeoWatcher.StopUpdatingHighAccuracyLocation();
                Log.LogMessage($"No waypoint founded for location {GeoWatcher.Location}");
                return Task.FromResult(new TollGeoStatusResult()
                {
                    TollGeolocationStatus = TollGeolocationStatus.OnTollRoad,
                    IsNeedToDoubleCheck = false
                });
            }
            else
            {
                return CheckNearestPoint(tollGeoStatus, nearestWaypoints);
            }
        }

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestTollPoint(GeoWatcher.Location, GeoDataService.GetAllExitTollPoints(WaypointChecker.TollRoad.Id)).FirstOrDefault()?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
    }
}

