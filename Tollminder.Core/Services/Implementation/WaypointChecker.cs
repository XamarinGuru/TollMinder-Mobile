using System;
using MvvmCross.Platform;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
	public class WaypointChecker : IWaypointChecker
	{
		private TollRoadWaypoint _waypoint;

		public virtual TollRoadWaypoint Waypoint {
			get { return _waypoint; }
			set {
				_waypoint = value;
				Mvx.Resolve<IDistanceChecker> ().UpdateDistance ();
			}
		}
	}
}

