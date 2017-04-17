using System.Linq;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Settings;
using Tollminder.Core.Models.GeoData;

namespace Tollminder.Core.Models.Statuses
{
    public class NotOnTollRoadStatus : BaseStatus
    {
        public override Task<TollGeoStatusResult> CheckStatus(TollGeoStatusResult tollGeoStatus)
        {
            Log.LogMessage(string.Format($"TRY TO FIND TOLLPOINT ENTRANCES FROM {SettingsService.WaypointLargeRadius * 1000} m"));

            if (tollGeoStatus?.TollPointWithDistance == null)
            {
#if REALEASE
                GeoWatcher.StopUpdatingHighAccuracyLocation();
#endif
                Log.LogMessage($"No waypoint founded for location {GeoWatcher.Location}");
                return Task.FromResult(new TollGeoStatusResult() { TollGeolocationStatus = TollGeolocationStatus.NotOnTollRoad });
            }
            else
            {
                foreach (var item in WaypointChecker.TollPointsInRadius)
                    Log.LogMessage($"FOUNDED WAYPOINT : {item.Name}, DISTANCE {item.Distance}");
#if REALEASE
                GeoWatcher.StartUpdatingHighAccuracyLocation();
#endif
                return Task.FromResult(new TollGeoStatusResult()
                {
                    TollGeolocationStatus = TollGeolocationStatus.NearTollRoadEntrance,
                    IsNeedToDoubleCheck = false
                });
            }
        }

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestTollPoint(GeoWatcher.Location, GeoDataService.GetAllEntranceTollPoints()).FirstOrDefault()?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
    }
}