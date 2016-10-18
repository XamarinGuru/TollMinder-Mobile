using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Utils;

namespace Tollminder.Core.Services.Implementation
{
	public class BatteryDrainService : IBatteryDrainService
	{
		readonly IGeoLocationWatcher _geoWatcher;
		readonly IDistanceChecker _distanceChecker;
		readonly IGeoDataService _geoDataService;
		readonly IStoredSettingsService _storedSettingsService;
        readonly IWaypointChecker _waypointChecker;
        readonly IMotionActivity _motionActivity;

		Timer _timer;

		Dictionary<Func<int, bool>, int> conditionsDictionary = new Dictionary<Func<int, bool>, int>()
		{
			{ x => x < 17,  0 },
			{ x => x >=17 && x < 31, 10 },
			{ x => x >= 31 && x < 96 , 15 },
			{ x => x >= 96 && x < 131 , 45 },
			{ x => x >= 131 && x < 193 , 90 },
			{ x => x >= 193, 120 } 
		};
	
		public BatteryDrainService()
		{
			_geoWatcher = Mvx.Resolve<IGeoLocationWatcher>();
			_distanceChecker = Mvx.Resolve<IDistanceChecker>();
			_geoDataService = Mvx.Resolve<IGeoDataService>();
			_storedSettingsService = Mvx.Resolve<IStoredSettingsService>();
            _waypointChecker = Mvx.Resolve<IWaypointChecker>();
            _motionActivity = Mvx.Resolve<IMotionActivity>();
		}

		public bool CheckGpsTrackingSleepTime(double distance)
        {
            var minutesOffset = conditionsDictionary.First(x => x.Key((int)distance)).Value;

			Log.LogMessage($"CheckGpsTrackingSleepTime : DIST = {distance}, MINUTESOFFSET {minutesOffset}");

			if (minutesOffset > 0)
			{
                Mvx.Resolve<INotificationSender>().SendLocalNotification($"{(int)distance} km to nearest WayPoint.", $"Stop GPS detecting for {minutesOffset} minutes");
				SetGpsTrackingSleepTime(minutesOffset);
				return true;
			}

			return false;
		}

		void TimerElapsed(object state)
		{
            if (_motionActivity.MotionType != MotionType.Still)
            {
                Log.LogMessage($"StartGeolocationWatcher from BatteryDrainService after {state} m of sleeping");
                _geoWatcher.StartGeolocationWatcher();
            }
            else
            {
                Log.LogMessage($"StartGeolocationWatcher was not started because we are still after {state} m of sleeping");
            }
            DisposeTimer();
		}

		public void SetGpsTrackingSleepTime(int minutes)
		{
            if (_timer != null)
                DisposeTimer();

			_storedSettingsService.SleepGPSDateTime = DateTime.Now.AddMinutes(minutes);
            Log.LogMessage($"Store SleepGPSDateTime {_storedSettingsService.SleepGPSDateTime} value to settings");
            _timer = new Timer(TimerElapsed, minutes, new TimeSpan(0, minutes, 0), new TimeSpan(0, minutes, 0));

			_geoWatcher.StopGeolocationWatcher();
            Log.LogMessage($"StopGeolocationWatcher for {minutes} m from BatteryDrainService");
		}

        private void DisposeTimer()
        {
            _timer.Cancel();
            _timer = null;
            Log.LogMessage($"Store SleepGPSDateTime MINDATE value to settings");
            _storedSettingsService.SleepGPSDateTime = DateTime.MinValue;
        }
	}
}

