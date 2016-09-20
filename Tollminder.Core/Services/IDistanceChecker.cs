using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IDistanceChecker
	{
		TollRoadWaypointWithDistance GetMostClosestWaypoint (GeoLocation center, IList<TollRoadWaypoint> points);
		ParallelQuery<TollRoadWaypoint> GetLocationsFromRadius (GeoLocation center, IList<TollRoadWaypoint> points);
		Task<ParallelQuery<TollRoadWaypoint>> GetLocationsFromRadiusAsync (GeoLocation center, IList<TollRoadWaypoint> points);
		TollRoadWaypoint GetLocationFromRadius (GeoLocation center, IList<TollRoadWaypoint> points);
		Task<TollRoadWaypoint> GetLocationFromRadiusAsync (GeoLocation center, IList<TollRoadWaypoint> points);
	}
}

