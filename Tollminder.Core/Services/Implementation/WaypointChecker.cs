using System;
using MvvmCross.Platform;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
	public class WaypointChecker : IWaypointChecker
	{
		private TollRoadWaypoint _waypoint;
		private IDistanceChecker _distanceChecker;

		public IDistanceChecker DistanceChecker {
			get {
				if (_distanceChecker == null) {
					_distanceChecker = Mvx.Resolve<IDistanceChecker> ();
				}
				return _distanceChecker;
			}
		}

		public virtual TollRoadWaypoint Waypoint {
			get { return _waypoint; }
			set {
				_waypoint = value;
				DistanceChecker.UpdateDistance ();
			}
		}
	}
}

