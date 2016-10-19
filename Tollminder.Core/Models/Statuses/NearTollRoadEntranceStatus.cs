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
            var insideTollPoint = WaypointChecker.DetectWeAreInsideSomeTollPoint(GeoWatcher.Location);

            if (insideTollPoint != null)
            {
                Log.LogMessage($"We are inside tollpoint {SettingsService.WaypointSmallRadius * 1000} radius");

                WaypointChecker.SetIgnoredChoiceTollPoint(insideTollPoint);

                if (await SpeechToTextService.AskQuestion($"Are you entering {insideTollPoint.Name} tollroad?"))
                {
                    WaypointChecker.SetEntrance(insideTollPoint);
                    WaypointChecker.SetIgnoredChoiceTollPoint(null);

                    if (insideTollPoint.WaypointAction == WaypointAction.EntranceAndExit)
                    {
                        WaypointChecker.SetExit(insideTollPoint);
                       
                        WaypointChecker.SetTollPointsInRadius(null);
                        await NotifyService.Notify("Bill was created");
                        WaypointChecker.CreateBill();
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

