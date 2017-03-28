using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadEntranceStatus : BaseStatus
    {
        public override bool CheckBatteryDrain()
        {
            return false;
        }

        public async override Task<TollGeolocationStatus> CheckStatus()
        {
            var location = GeoWatcher.Location;
            var waypoints = GeoDataService.FindNearestEntranceTollPoints(location);

            WaypointChecker.SetTollPointsInRadius(waypoints);

            if (waypoints.Count == 0)
                return TollGeolocationStatus.NotOnTollRoad;

            var insideTollPoint = WaypointChecker.DetectWeAreInsideSomeTollPoint(location);

            if (insideTollPoint != null)
            {
                double radius = insideTollPoint.Radius != 0 ? insideTollPoint.Radius / 1000 : SettingsService.WaypointSmallRadius * 1000;
                Log.LogMessage($"We are inside tollpoint {radius} radius");

                WaypointChecker.SetIgnoredChoiceTollPoint(insideTollPoint);

                if (WaypointChecker.TollPointsInRadius.Count == 1)
                    GeoWatcher.StopUpdatingHighAccuracyLocation();

                if (await SpeechToTextService.AskQuestionAsync($"Are you entering {insideTollPoint.Name} tollroad?"))
                {
                    WaypointChecker.SetEntrance(insideTollPoint);

                    if (insideTollPoint.WaypointAction == WaypointAction.Bridge)
                    {
                        WaypointChecker.SetExit(insideTollPoint);

                        WaypointChecker.SetTollPointsInRadius(null);
                        await NotifyService.NotifyAsync("Bill was created");
                        WaypointChecker.ClearData();
                        return TollGeolocationStatus.NotOnTollRoad;
                    }
                    else
                        return TollGeolocationStatus.OnTollRoad;
                }
                else
                {
                    return TollGeolocationStatus.NotOnTollRoad;
                }
            }
            else
            {
                return TollGeolocationStatus.NearTollRoadEntrance;
            }
        }
    }
}

