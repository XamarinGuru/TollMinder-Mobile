using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadExitStatus : BaseStatus
    {
        public override async Task<TollGeolocationStatus> CheckStatus()
        {
            var insideTollPoint = WaypointChecker.DetectWeAreInsideSomeTollPoint(GeoWatcher.Location);

            if (insideTollPoint != null)
            {
                Log.LogMessage($"We are inside tollpoint {SettingsService.WaypointSmallRadius * 1000} radius");

                WaypointChecker.SetIgnoredChoiceTollPoint(insideTollPoint);

                if (await SpeechToTextService.AskQuestion($"Are you entering {insideTollPoint.Name} tollroad?"))
                {
                    WaypointChecker.SetExit(insideTollPoint);
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

                    WaypointChecker.SetTollPointsInRadius(null);

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

