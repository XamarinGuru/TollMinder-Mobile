using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
	public class WaypointChecker : IWaypointChecker
	{
		private TollRoadWaypoint _waypoint;
		private readonly IDistanceChecker _distanceChecker;

		public WaypointChecker (IDistanceChecker distanceChecker)
		{
			this._distanceChecker = distanceChecker;
		}

		public virtual TollRoadWaypoint Waypoint {
			get { return _waypoint; }
			set {
				_waypoint = value;
				_distanceChecker.UpdateDistance ();
			}
		}
	}
}

