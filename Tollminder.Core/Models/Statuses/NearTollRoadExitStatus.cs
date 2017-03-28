using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadExitStatus : BaseStatus
    {
        public override async Task<TollGeolocationStatus> CheckStatus()
        {
            var location = GeoWatcher.Location;
            var waypoints = GeoDataService.FindNearestExitTollPoints(location);

            WaypointChecker.SetTollPointsInRadius(waypoints);

            if (waypoints.Count == 0)
                return TollGeolocationStatus.OnTollRoad;

            var insideTollPoint = WaypointChecker.DetectWeAreInsideSomeTollPoint(location);

            if (insideTollPoint != null)
            {
                double radius = insideTollPoint.Radius != 0 ? insideTollPoint.Radius / 1000 : SettingsService.WaypointSmallRadius * 1000;
                Log.LogMessage($"We are inside tollpoint {radius} radius");

                WaypointChecker.SetIgnoredChoiceTollPoint(insideTollPoint);

                if (WaypointChecker.TollPointsInRadius.Count == 1)
                    GeoWatcher.StopUpdatingHighAccuracyLocation();

                if (await SpeechToTextService.AskQuestionAsync($"Are you exiting from {insideTollPoint.Name} tollroad?"))
                {
                    WaypointChecker.SetExit(insideTollPoint);
                    WaypointChecker.SetTollPointsInRadius(null);
                    WaypointChecker.SetIgnoredChoiceTollPoint(null);

                    if (WaypointChecker.Exit != null)
                    {
                        await NotifyService.NotifyAsync("Bill was created");

                        var duration = WaypointChecker.TripDuration;

                        if (duration.Hours > 0)
                            await NotifyService.NotifyAsync($"Trip duration is {duration.Hours} hours {duration.Minutes} minutes {duration.Seconds} seconds");
                        else
                            await NotifyService.NotifyAsync($"Trip duration is {duration.Minutes} minutes {duration.Seconds} seconds");

                        WaypointChecker.ClearData();
                    }
                    else
                    {
                        await NotifyService.NotifyAsync("Bill was not created. You didn't enter any exit");
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

