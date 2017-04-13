using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Settings;
using MvvmCross.Platform;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Models.GeoData;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadExitStatus : BaseStatus
    {
        public override async Task<TollGeoStatusResult> CheckStatus(TollGeolocationStatus tollGeoStatus)
        {
            var location = GeoWatcher.Location;
            var waypoints = GeoDataService.FindNearestExitTollPoints(location);

            WaypointChecker.SetTollPointsInRadius(waypoints);

            if (waypoints.Count == 0)
                return new TollGeoStatusResult()
                {
                    TollGeolocationStatus = TollGeolocationStatus.OnTollRoad,
                    IsNeedToDoubleCheck = false
                };

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
                    SaveTripProgress();
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

                    return new TollGeoStatusResult()
                    {
                        TollGeolocationStatus = TollGeolocationStatus.NotOnTollRoad,
                        IsNeedToDoubleCheck = false
                    };
                }
                else
                {
                    return new TollGeoStatusResult()
                    {
                        TollGeolocationStatus = TollGeolocationStatus.OnTollRoad,
                        IsNeedToDoubleCheck = false
                    };
                }
            }
            else
            {
                return new TollGeoStatusResult()
                {
                    TollGeolocationStatus = TollGeolocationStatus.NearTollRoadExit,
                    IsNeedToDoubleCheck = false
                };
            }
        }

        public override bool CheckBatteryDrain()
        {
            return false;
        }

        private void SaveTripProgress()
        {
            Mvx.Resolve<IPaymentProcessing>().TripCompletedAsync(new PaymentData.TripCompleted()
            {
                StartWayPointId = WaypointChecker.Entrance.Id,
                EndWayPointId = WaypointChecker.Exit.Id,
                TollRoadId = WaypointChecker.Exit.TollRoadId,
                UserId = Mvx.Resolve<IStoredSettingsService>().ProfileId
            });
        }
    }
}

