using System.Threading.Tasks;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadExitStatus : BaseStatus
    {
        bool? _previousLocationIsCloser;

        public override async Task<TollGeolocationStatus> CheckStatus()
        {
            var isCloserToNextWaypoint = WaypointChecker.IsCloserToNextWaypoint(GeoWatcher.Location);

            Log.LogMessage($"Is Closer to {WaypointChecker.CurrentWaypoint?.Name} waypoint : {isCloserToNextWaypoint}");

            if (isCloserToNextWaypoint)
            {
                Log.LogMessage(string.Format("DISTANCE BETWEEN CAR AND WAYPOINT IS CLOSER"));

                if (WaypointChecker.IsAtNextWaypoint(GeoWatcher.Location))
                {
                    Log.LogMessage($"We are inside waypoint 30m radius");

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

