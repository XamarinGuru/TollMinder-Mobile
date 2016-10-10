﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core.Models.Statuses
{
    public class NearTollRoadExitStatus : BaseStatus
    {
        bool _previousLocationIsNotCloser;

        public override async Task<TollGeolocationStatus> CheckStatus()
        {
            var isCloserToNextWaypoint = WaypointChecker.IsCloserToNextWaypoint(GeoWatcher.Location);

            Log.LogMessage($"Is Closer to {WaypointChecker.CurrentWaypoint?.Name} waypoint : {isCloserToNextWaypoint}");

            if (isCloserToNextWaypoint)
            {
                _previousLocationIsNotCloser = !isCloserToNextWaypoint;
                Log.LogMessage(string.Format("DISTANCE BETWEEN CAR AND WAYPOINT IS CLOSER"));

                if (WaypointChecker.IsAtNextWaypoint(GeoWatcher.Location))
                {
                    Log.LogMessage($"We are inside waypoint 30m radius");

                    WaypointChecker.SetExit(WaypointChecker.CurrentWaypoint);
                    GeoWatcher.StopUpdatingHighAccuracyLocation();

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

                        return TollGeolocationStatus.NotOnTollRoad;
                    }
                    else
                    {
                        WaypointChecker.SetIgnoredChoiceWaypoint(WaypointChecker.CurrentWaypoint);
                        return TollGeolocationStatus.OnTollRoad;
                    }
                }
            }

            return TollGeolocationStatus.OnTollRoad;
        }

        public override bool CheckBatteryDrain()
        {
            return false;
        }
    }
}

