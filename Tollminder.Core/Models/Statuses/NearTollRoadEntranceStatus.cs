using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadEntranceStatus : BaseStatus
    {
        bool? _previousLocationIsCloser = null;

        public override bool CheckBatteryDrain()
        {
            return false;
        }

        public async override Task<TollGeolocationStatus> CheckStatus()
        {
            var isCloserToNextWaypoint = WaypointChecker.IsCloserToNextWaypoint(GeoWatcher.Location);

            Log.LogMessage($"Are we closer to {WaypointChecker.CurrentWaypoint?.Name} waypoint : {isCloserToNextWaypoint}, previous state : {_previousLocationIsCloser}");

            if (isCloserToNextWaypoint)
            {
                if (WaypointChecker.IsAtNextWaypoint(GeoWatcher.Location))
                {
                    Log.LogMessage($"We are inside waypoint {SettingsService.WaypointSmallRadius * 1000} radius");

                    GeoWatcher.StopUpdatingHighAccuracyLocation();
                    _previousLocationIsCloser = null;
                    if (await SpeechToTextService.AskQuestion($"Are you entering {WaypointChecker.CurrentWaypoint.Name} tollroad?"))
                    {
                        WaypointChecker.SetEntrance(WaypointChecker.CurrentWaypoint);

                        if (WaypointChecker.CurrentWaypoint.WaypointAction == WaypointAction.EntranceAndExit)
                        {
                            WaypointChecker.SetExit(WaypointChecker.CurrentWaypoint);
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

