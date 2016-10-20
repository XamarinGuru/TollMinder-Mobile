using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadExitStatus : BaseStatus
    {
        public override async Task<TollGeolocationStatus> CheckStatus()
        {
            var location = GeoWatcher.Location;
            var waypoints = DataService.FindNearestEntranceTollPoints(location, WaypointChecker.IgnoredChoiceTollPoint);

            WaypointChecker.SetTollPointsInRadius(waypoints);

            var insideTollPoint = WaypointChecker.DetectWeAreInsideSomeTollPoint(location);

            if (insideTollPoint != null)
            {
                Log.LogMessage($"We are inside tollpoint {SettingsService.WaypointSmallRadius * 1000} radius");

                WaypointChecker.SetIgnoredChoiceTollPoint(insideTollPoint);

                if (WaypointChecker.TollPointsInRadius.Count == 1)
                    GeoWatcher.StopUpdatingHighAccuracyLocation();

                if (await SpeechToTextService.AskQuestion($"Are you entering {insideTollPoint.Name} tollroad?"))
                {
                    WaypointChecker.SetExit(insideTollPoint);
                    WaypointChecker.SetTollPointsInRadius(null);
                    WaypointChecker.SetIgnoredChoiceTollPoint(null);

                    if (WaypointChecker.Exit != null)
                    {
                        await NotifyService.Notify("Bill was created");
                        WaypointChecker.CreateBill();
                    }
                    else
                    {
                        await NotifyService.Notify("Bill was not created. You didn't enter any exit");
                    }

                    return TollGeolocationStatus.NotOnTollRoad;
                }
                else
                {
                    return TollGeolocationStatus.OnTollRoad;
                }
            }
            else
            {
                return TollGeolocationStatus.NearTollRoadExit;
            }
        }

        public override bool CheckBatteryDrain()
        {
            return false;
        }
    }
}

