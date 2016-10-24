using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

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
            var waypoints = DataService.FindNearestEntranceTollPoints(location);

            WaypointChecker.SetTollPointsInRadius(waypoints);

            if(waypoints.Count == 0)
                return TollGeolocationStatus.NotOnTollRoad;

            var insideTollPoint = WaypointChecker.DetectWeAreInsideSomeTollPoint(location);

            if (insideTollPoint != null)
            {
                Log.LogMessage($"We are inside tollpoint {SettingsService.WaypointSmallRadius * 1000} radius");

                WaypointChecker.SetIgnoredChoiceTollPoint(insideTollPoint);

                if (WaypointChecker.TollPointsInRadius.Count == 1)
                    GeoWatcher.StopUpdatingHighAccuracyLocation();

                if (await SpeechToTextService.AskQuestion($"Are you entering {insideTollPoint.Name} tollroad?"))
                {
                    WaypointChecker.SetEntrance(insideTollPoint);

                    if (insideTollPoint.WaypointAction == WaypointAction.EntranceAndExit)
                    {
                        WaypointChecker.SetExit(insideTollPoint);
                       
                        WaypointChecker.SetTollPointsInRadius(null);
                        await NotifyService.Notify("Bill was created");
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

