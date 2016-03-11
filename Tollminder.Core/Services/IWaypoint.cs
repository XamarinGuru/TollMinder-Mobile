using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IWaypointChecker
	{
		TollRoadWaypoint Waypoint { get; set; }
	}
}

