using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models.GeoData;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadEntranceStatus : BaseStatus
    {
        public override bool CheckBatteryDrain()
        {
            return false;
        }

        public async override Task<TollGeoStatusResult> CheckStatus(TollGeolocationStatus tollGeoStatus)
        {
            var location = GeoWatcher.Location;
            var waypoints = GeoDataService.FindNearestEntranceTollPoints(location);

            WaypointChecker.SetTollPointsInRadius(waypoints);

            if (waypoints.Count == 0)
                return new TollGeoStatusResult()
                {
                    TollGeolocationStatus = TollGeolocationStatus.NotOnTollRoad,
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

                if (await SpeechToTextService.AskQuestionAsync($"Are you entering {insideTollPoint.Name} tollroad?"))
                {
                    WaypointChecker.SetEntrance(insideTollPoint);

                    if (insideTollPoint.WaypointAction == WaypointAction.Bridge)
                    {
                        WaypointChecker.SetExit(insideTollPoint);
                        SaveTripProgress();
                        WaypointChecker.SetTollPointsInRadius(null);
                        await NotifyService.NotifyAsync("Bill was created");
                        WaypointChecker.ClearData();
                        return new TollGeoStatusResult()
                        {
                            TollGeolocationStatus = TollGeolocationStatus.NotOnTollRoad,
                            IsNeedToDoubleCheck = false
                        };
                    }
                    else
                        return new TollGeoStatusResult()
                        {
                            TollGeolocationStatus = TollGeolocationStatus.OnTollRoad,
                            IsNeedToDoubleCheck = false
                        };
                }
                else
                {
                    return new TollGeoStatusResult()
                    {
                        TollGeolocationStatus = TollGeolocationStatus.NotOnTollRoad,
                        IsNeedToDoubleCheck = false
                    };
                }
            }
            else
            {
                return new TollGeoStatusResult()
                {
                    TollGeolocationStatus = TollGeolocationStatus.NearTollRoadEntrance,
                    IsNeedToDoubleCheck = false
                };
            }
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

