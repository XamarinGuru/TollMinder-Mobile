using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;

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
            var isCloserToNextWaypoint = WaypointChecker.IsCloserToNextWaypoint(GeoWatcher.Location);

            Log.LogMessage($"Is closer to {WaypointChecker.CurrentWaypoint?.Name} entrance waypoint : {isCloserToNextWaypoint}");

            if (isCloserToNextWaypoint)
            {
                if (WaypointChecker.IsAtNextWaypoint(GeoWatcher.Location))
                {
                    Log.LogMessage($"We are inside waypoint 30m radius");

                    GeoWatcher.StopUpdatingHighAccuracyLocation();

                    if (await SpeechToTextService.AskQuestion($"Are you entering {WaypointChecker.CurrentWaypoint.Name} tollroad?"))
                    {
                        WaypointChecker.SetEntrance(WaypointChecker.CurrentWaypoint);
                        return TollGeolocationStatus.OnTollRoad;
                    }
                    else
                    {
                        return TollGeolocationStatus.NotOnTollRoad;
                    }
                }
            }

            return TollGeolocationStatus.NearTollRoadEntrance;
        }
    }
}

