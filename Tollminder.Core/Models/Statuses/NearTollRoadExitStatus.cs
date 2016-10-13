using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadExitStatus : BaseStatus
    {
        bool? _previousLocationIsCloser;

        public override async Task<TollGeolocationStatus> CheckStatus()
        {
            var isCloserToNextWaypoint = WaypointChecker.IsCloserToNextWaypoint(GeoWatcher.Location);
            string status = (isCloserToNextWaypoint) ? "closer" : "not closer";
            Log.LogMessage($"Distance to {WaypointChecker.CurrentWaypoint?.Name} waypoint is {WaypointChecker.DistanceToNextWaypoint} ({status})");

            if (isCloserToNextWaypoint)
            {
                Log.LogMessage(string.Format("DISTANCE BETWEEN CAR AND WAYPOINT IS CLOSER"));

                if (WaypointChecker.IsAtNextWaypoint(GeoWatcher.Location))
                {
                    Log.LogMessage($"We are inside waypoint {SettingsService.WaypointSmallRadius * 1000} m radius");

                    WaypointChecker.SetExit(WaypointChecker.CurrentWaypoint);
                    GeoWatcher.StopUpdatingHighAccuracyLocation();
                    _previousLocationIsCloser = null;

                    if (await SpeechToTextService.AskQuestion($"Are you exiting {WaypointChecker.Exit.Name} the tollroad?"))
                    {
                        if (WaypointChecker.Exit != null)
                        {
                            await NotifyService.Notify("Bill was created");
                            WaypointChecker.CreateBill();
                        }
                        else
                        {
                            await NotifyService.Notify("Bill was not created. You didn't enter any exit");
                        }

                        WaypointChecker.SetCurrentWaypoint(null);
                        WaypointChecker.SetIgnoredChoiceWaypoint(null);

                        return TollGeolocationStatus.NotOnTollRoad;
                    }
                    else
                    {
                        WaypointChecker.SetIgnoredChoiceWaypoint(WaypointChecker.CurrentWaypoint);
                        return TollGeolocationStatus.OnTollRoad;
                    }
                }
            }
            else
            {
                if ((_previousLocationIsCloser != null && !(bool)_previousLocationIsCloser))
                {
                    GeoWatcher.StopUpdatingHighAccuracyLocation();
                    WaypointChecker.SetIgnoredChoiceWaypoint(WaypointChecker.CurrentWaypoint);
                    return TollGeolocationStatus.OnTollRoad;
                }
            }

            _previousLocationIsCloser = isCloserToNextWaypoint;

            return TollGeolocationStatus.NearTollRoadExit;
        }

        public override bool CheckBatteryDrain()
        {
            return false;
        }
    }
}

