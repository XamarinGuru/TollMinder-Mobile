using System;
using Tollminder.Core.Models;
using System.Linq;
using System.Collections.Generic;

namespace Tollminder.Core.Services
{
	public interface IGeoDataService
	{
		ParallelQuery<TollRoadWaypoint> FindNearGeoLocations (GeoLocation center);
		TollRoadWaypoint FindNearGeoLocation (GeoLocation center);
		TollRoadWaypoint FindNearGeoLocation (GeoLocation center, WaypointAction status);
		List<TollRoadWaypoint> GetWaypoints();
		void Update (TollRoadWaypoint geoLocation);
		void Insert (TollRoadWaypoint geoLocation);
		void Delete (TollRoadWaypoint geoLocation);
		int Count { get; }
	}
}

