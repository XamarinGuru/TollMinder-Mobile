using System;
using Tollminder.Core.Models;
using System.Linq;

namespace Tollminder.Core.Services
{
	public interface IGeoDataService
	{
		ParallelQuery<TollRoadWaypoint> FindNearGeoLocations (GeoLocation center);
		TollRoadWaypoint FindNearGeoLocation (GeoLocation center);
		TollRoadWaypoint FindNearGeoLocation (GeoLocation center, WaypointAction status);
		void Update (TollRoadWaypoint geoLocation);
		void Insert (TollRoadWaypoint geoLocation);
		void Delete (TollRoadWaypoint geoLocation);
		int Count { get; }
	}
}

