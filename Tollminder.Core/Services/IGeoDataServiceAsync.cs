using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Models;
using System.Linq;

namespace Tollminder.Core.Services
{
	public interface IGeoDataServiceAsync
	{
		Task<ParallelQuery<TollRoadWaypoint>> FindNearGeoLocationsAsync (GeoLocation center);
		Task<TollRoadWaypoint> FindNearGeoLocationAsync (GeoLocation center);
		Task UpdateAsync (TollRoadWaypoint geoLocation);
		Task InsertAsync (TollRoadWaypoint geoLocation);
		Task DeleteAsync (TollRoadWaypoint geoLocation);
		Task<int> CountAsync { get; }
	}
}

