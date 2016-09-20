using System;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Services;

namespace Tollminder.Core.Models.Statuses
{
	public abstract class BaseStatus 
	{
		IGeoLocationWatcher _geoWatcher;
		protected IGeoLocationWatcher GeoWatcher 
		{
			get 
			{
				return _geoWatcher ?? (_geoWatcher = Mvx.Resolve<IGeoLocationWatcher>());
			}
		}

		IMotionActivity _motionActivity;
		protected IMotionActivity MotionActivity 
		{
			get 
			{
				return _motionActivity ?? (_motionActivity = Mvx.Resolve<IMotionActivity>());
			}
		}

		IDistanceChecker _distanceChecker;
		protected IDistanceChecker DistanceChecker 
		{
			get 
			{
				return _distanceChecker ?? (_distanceChecker = Mvx.Resolve<IDistanceChecker>());
			}
		}

		IWaypointChecker _waypointChecker;
		protected IWaypointChecker WaypointChecker {
			get {
				if (_waypointChecker == null) {
					_waypointChecker = Mvx.Resolve<IWaypointChecker> ();
				}
				return _waypointChecker;
			}
		}

		IGeoDataService _dataService;
		protected IGeoDataService DataService 
		{
			get 
			{
				return _dataService ?? (_dataService = Mvx.Resolve<IGeoDataService>());
			}
		}

		INotifyService _notifyService;
		protected INotifyService NotifyService 
		{
			get 
			{
				return _notifyService ?? (_notifyService = Mvx.Resolve<INotifyService>());
			}
		}

		ISpeechToTextService _speechToTextService;
		protected ISpeechToTextService SpeechToTextService
		{
			get
			{
				return _speechToTextService ?? (_speechToTextService = Mvx.Resolve<ISpeechToTextService>());
			}
		}

		IBatteryDrainService _batteryDrainService;
		protected IBatteryDrainService BatteryDrainService
		{
			get
			{
				return _batteryDrainService ?? (_batteryDrainService = Mvx.Resolve<IBatteryDrainService>());
			}
		}

		public abstract Task<TollGeolocationStatus> CheckStatus ();
	}
}

