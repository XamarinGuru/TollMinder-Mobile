using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class OnTollRoadStatus : BaseStatus
	{
        const double WaypointAreaDistanceRequired = 0.05;
        bool _previousLocationIsNotCloser;

		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
            var isCloserToNextWaypoint = WaypointChecker.IsCloserToNextWaypoint(GeoWatcher.Location);

            Log.LogMessage($"Is Closer to {WaypointChecker.NextWaypoint?.Name} waypoint : {isCloserToNextWaypoint}");

            if (isCloserToNextWaypoint)
            {
                _previousLocationIsNotCloser = !isCloserToNextWaypoint;
                Log.LogMessage(string.Format("DISTANCE BETWEEN CAR AND WAYPOINT IS CLOSER"));

                var flag = WaypointChecker.DistanceToNextWaypoint - WaypointAreaDistanceRequired < double.Epsilon;

                Log.LogMessage($"Check if distance from current location to next waypoint is less than {WaypointAreaDistanceRequired * 1000} m. [{flag}]");

                if (flag)
                {
                    _previousLocationIsNotCloser = false;
                    GeoWatcher.StartUpdatingHighAccuracyLocation();
                    return TollGeolocationStatus.NearTollRoadExit;
                }
            }
            else
            {
                Log.LogMessage(string.Format("DISTANCE BETWEEN CAR AND WAYPOINT IS NOT CLOSER"));
                if (_previousLocationIsNotCloser)
                {
                    if (await SpeechToTextService.AskQuestion($"Are you exiting {WaypointChecker.NextWaypoint.Name} the tollroad?"))
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
                        GeoWatcher.StopUpdatingHighAccuracyLocation();

                        return TollGeolocationStatus.NotOnTollRoad;
                    }
                    else
                        _previousLocationIsNotCloser = false;
                }
                else
                    _previousLocationIsNotCloser = !isCloserToNextWaypoint;
            }


            Log.LogMessage($"PreviousLocationIsNotCloser = {_previousLocationIsNotCloser}");
            return TollGeolocationStatus.OnTollRoad;
		}

        public override bool CheckBatteryDrain()
        {
            var distance = DistanceChecker.GetMostClosestWaypoint(GeoWatcher.Location, new List<TollRoadWaypoint>() { WaypointChecker.NextWaypoint })?.Distance ?? 0;
            return BatteryDrainService.CheckGpsTrackingSleepTime(distance);
        }
	}
}

