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

            Log.LogMessage($"Is closer to {WaypointChecker.CurrentWaypoint?.Name} entrance waypoint : {isCloserToNextWaypoint}");

            if (isCloserToNextWaypoint)
            {
                if (WaypointChecker.IsAtNextWaypoint(GeoWatcher.Location))
                {
                    Log.LogMessage($"We are inside waypoint {SettingsService.WaypointSmallRadius * 1000} m radius");

                    GeoWatcher.StopUpdatingHighAccuracyLocation();
                    _previousLocationIsCloser = null;

                    if (await SpeechToTextService.AskQuestion($"Are you entering {WaypointChecker.CurrentWaypoint.Name} tollroad?"))
                    {
                        WaypointChecker.SetEntrance(WaypointChecker.CurrentWaypoint);
                        WaypointChecker.SetCurrentWaypoint(null);
                        WaypointChecker.SetIgnoredChoiceWaypoint(null);

                        return TollGeolocationStatus.OnTollRoad;
                    }
                    else
                    {
                        WaypointChecker.SetIgnoredChoiceWaypoint(WaypointChecker.CurrentWaypoint);
                        return TollGeolocationStatus.NotOnTollRoad;
                    }
                }
            }
            else
            {
                if ((_previousLocationIsCloser != null && !(bool)_previousLocationIsCloser))
                {
                    WaypointChecker.SetIgnoredChoiceWaypoint(WaypointChecker.CurrentWaypoint);
                    return TollGeolocationStatus.NotOnTollRoad;
                }
            }

            _previousLocationIsCloser = isCloserToNextWaypoint;

            return TollGeolocationStatus.NearTollRoadEntrance;
        }
    }
}

