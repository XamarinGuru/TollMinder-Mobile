using System;
using MvvmCross.Platform;
using Tollminder.Core.Services;

namespace Tollminder.Core.Models.Statuses
{
	public abstract class BaseStatus 
	{
		IGeoLocationWatcher _geoWatcher;
		protected IGeoLocationWatcher GeoWatcher {
			get {
				if (_geoWatcher == null) {
					_geoWatcher = Mvx.Resolve<IGeoLocationWatcher> ();
				}
				return _geoWatcher;
			}
		}

		IMotionActivity _motionActivity;
		protected IMotionActivity MotionActivity {
			get {
				if (_motionActivity == null) {
					_motionActivity = Mvx.Resolve<IMotionActivity> ();
				}
				return _motionActivity;
			}
		}

		IDistanceChecker _distanceChecker;
		protected IDistanceChecker DistanceChecker {
			get {
				if (_distanceChecker == null) {
					_distanceChecker = Mvx.Resolve<IDistanceChecker> ();
				}
				return _distanceChecker;
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
		protected IGeoDataService DataService {
			get {
				if (_dataService == null) {
					_dataService = Mvx.Resolve<IGeoDataService> ();
				}
				return _dataService;
			}
		}

		INotifyService _notifyService;
		protected INotifyService NotifyService {
			get {
				if (_notifyService == null) {
					_notifyService = Mvx.Resolve<INotifyService> ();
				}
				return _notifyService;
			}
		}

		public abstract TollGeolocationStatus CheckStatus ();
	}
}

