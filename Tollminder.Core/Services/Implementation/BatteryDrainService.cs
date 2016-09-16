using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Utils;

namespace Tollminder.Core.Services.Implementation
{
	public class BatteryDrainService : IBatteryDrainService
	{
		readonly IGeoLocationWatcher _geoWatcher;
		readonly IDistanceChecker _distanceChecker;
		readonly IGeoDataService _geoDataService;

		Timer _timer;

		Dictionary<Func<int, bool>, int> conditionsDictionary = new Dictionary<Func<int, bool>, int>()
		{
			{ x => x < 17,  0 },
			{ x => x >=17 && x < 31, 1 },
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
		}

		public void CheckGpsTrackingSleepTime()
		{ 
			double distance = _distanceChecker.GetMostClosestWaypoint(_geoWatcher.Location, (_geoDataService.GetWaypoints()))?.Distance ?? 0;
			var minutesOffset = conditionsDictionary.First(x => x.Key((int)distance)).Value;

			Log.LogMessage($"CheckGpsTrackingSleepTime : DIST = {distance}, MINUTESOFFSET {minutesOffset}");

			if (minutesOffset > 0)
			{
				Mvx.Resolve<INotifyService>().Notify($"{(int)distance} km to nearest WayPoint. Stop GPS detecting for {minutesOffset} minutes");
				SetGpsTrackingSleepTime(minutesOffset);
			}
		}

		void TimerElapsed(object state = null)
		{
			Mvx.Resolve<INotifyService>().Notify($"Start GPS");
			_geoWatcher.StartGeolocationWatcher();
			_timer.Cancel();
		}

		public void SetGpsTrackingSleepTime(int minutes)
		{
			if (_timer != null)
			{
				_timer.Cancel();
				_timer = null;
			}

			_timer = new Timer(TimerElapsed, null, new TimeSpan(0, minutes, 0), new TimeSpan(0, minutes, 0));

			_geoWatcher.StopGeolocationWatcher();
		}
	}
}

