using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Models;
using System.Linq;

namespace Tollminder.Core.Services
{
	public interface IGeoDataServiceAsync
	{
		Task<ParallelQuery<GeoLocation>> FindNearGeoLocationsAsync (GeoLocation center);
		Task UpdateAsync (GeoLocation geoLocation);
		Task InsertAsync (GeoLocation geoLocation);
		Task DeleteAsync (GeoLocation geoLocation);
		Task<int> CountAsync { get; }
	}
}

