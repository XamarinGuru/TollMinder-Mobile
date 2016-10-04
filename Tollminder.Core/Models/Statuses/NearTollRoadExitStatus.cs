using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadExitStatus : BaseStatus
    {
        public override async Task<TollGeolocationStatus> CheckStatus()
        {
            var isCloserToNextWaypoint = WaypointChecker.IsCloserToNextWaypoint(GeoWatcher.Location);

            Log.LogMessage($"Is Closer to {WaypointChecker.NextWaypoint?.Name} waypoint : {isCloserToNextWaypoint}");

            if (isCloserToNextWaypoint)
            {
                Log.LogMessage(string.Format("DISTANCE BETWEEN CAR AND WAYPOINT IS CLOSER"));

                if (WaypointChecker.IsAtNextWaypoint(GeoWatcher.Location))
                {
                    await NotifyService.Notify(string.Format("You are entered to {0}", WaypointChecker.NextWaypoint.Name));
                    WaypointChecker.SetExit(WaypointChecker.NextWaypoint);
                    var waypoint = DataService.FindNextExitWaypoint(WaypointChecker.NextWaypoint);

                    if (waypoint != null)
                        WaypointChecker.SetNextWaypoint(waypoint);
                    else
                    {
                        await NotifyService.Notify("You've reached last waypoint of this road");
                        await NotifyService.Notify("Bill was created");
                        WaypointChecker.SetCurrentWaypoint(null);
                        WaypointChecker.CreateBill();
                        GeoWatcher.StopUpdatingHighAccuracyLocation();
                        return TollGeolocationStatus.NotOnTollRoad;
                    }
                }

                return TollGeolocationStatus.NearTollRoadExit;
            }
            else
            {
                Log.LogMessage("Need ask about exit");
                if (await SpeechToTextService.AskQuestion($"Are you exiting {WaypointChecker.NextWaypoint.Name} the tollroad?"))
                {
                    await NotifyService.Notify("Bill was created");
                    WaypointChecker.SetCurrentWaypoint(null);
                    WaypointChecker.CreateBill();
                    GeoWatcher.StopUpdatingHighAccuracyLocation();
                    return TollGeolocationStatus.NotOnTollRoad;
                }
                else
                {
                    return TollGeolocationStatus.OnTollRoad;
                }
            }
        }

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestWaypoint(GeoWatcher.Location, new List<TollRoadWaypoint>() { WaypointChecker.NextWaypoint })?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
    }
}

