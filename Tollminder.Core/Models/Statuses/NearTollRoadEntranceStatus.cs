using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadEntranceStatus : BaseStatus
    {
        bool? _previousLocationIsCloser;

        public override bool CheckBatteryDrain()
        {
            return false;
        }

        public async override Task<TollGeolocationStatus> CheckStatus()
        {
            var isCloserToNextWaypoint = WaypointChecker.IsCloserToNextWaypoint(GeoWatcher.Location);
            string status = (isCloserToNextWaypoint) ? "closer" : "not closer";
            Log.LogMessage($"Distance to {WaypointChecker.CurrentTollPoint?.Name} waypoint is {WaypointChecker.DistanceToNextWaypoint} ({status})");

            if (isCloserToNextWaypoint)
            {
                if (WaypointChecker.IsAtNextWaypoint(GeoWatcher.Location))
                {
                    Log.LogMessage($"We are inside waypoint {SettingsService.WaypointSmallRadius * 1000} radius");

                    GeoWatcher.StopUpdatingHighAccuracyLocation();
                    _previousLocationIsCloser = null;
                    if (await SpeechToTextService.AskQuestion($"Are you entering {WaypointChecker.CurrentTollPoint.Name} tollroad?"))
                    {
                        WaypointChecker.SetEntrance(WaypointChecker.CurrentTollPoint);
                        WaypointChecker.SetCurrentTollPoint(null);
                        WaypointChecker.SetIgnoredChoiceTollPoint(null);

                        if (WaypointChecker.CurrentTollPoint.WaypointAction == WaypointAction.EntranceAndExit)
                        {
                            WaypointChecker.SetExit(WaypointChecker.CurrentTollPoint);
                            await NotifyService.Notify("Bill was created");
                            WaypointChecker.CreateBill();
                            return TollGeolocationStatus.NotOnTollRoad;
                        }
                        else
                            return TollGeolocationStatus.OnTollRoad;
                    }
                    else
                    {
                        WaypointChecker.SetIgnoredChoiceTollPoint(WaypointChecker.CurrentTollPoint);
                        return TollGeolocationStatus.NotOnTollRoad;
                    }
                }
            }
            else
            {
                if ((_previousLocationIsCloser != null && !(bool)_previousLocationIsCloser))
                {
                    GeoWatcher.StopUpdatingHighAccuracyLocation();
                    WaypointChecker.SetIgnoredChoiceTollPoint(WaypointChecker.CurrentTollPoint);
                    return TollGeolocationStatus.NotOnTollRoad;
                }
            }

            _previousLocationIsCloser = isCloserToNextWaypoint;

            return TollGeolocationStatus.NearTollRoadEntrance;
        }
    }
}

