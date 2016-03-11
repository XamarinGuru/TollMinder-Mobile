using System;
using MvvmCross.Platform;
using Tollminder.Core.Services;

namespace Tollminder.Core.Models.Statuses
{
	public abstract class BaseStatus
	{
		protected IGeoLocationWatcher GeoWatcher { get; } = Mvx.Resolve<IGeoLocationWatcher> ();
		protected IMotionActivity MotionActivity { get; } = Mvx.Resolve<IMotionActivity> ();
		protected IDistanceChecker DistanceChecker { get; } = Mvx.Resolve<IDistanceChecker> ();
		protected IWaypointChecker WaypointChecker { get; } = Mvx.Resolve<IWaypointChecker> ();
		protected IGeoDataService DataService { get; } = Mvx.Resolve<IGeoDataService> ();

		public abstract TollGeolocationStatus CheckStatus ();
	}
}

