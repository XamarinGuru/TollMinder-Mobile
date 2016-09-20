using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IWaypointChecker
	{
		TollRoadWaypoint CurrentWaypoint { get; set; }
		TollRoadWaypoint Entrance { get; set;}
		TollRoadWaypoint Exit { get; set;}
	}
}

