using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IDistanceChecker
	{
        TollPointWithDistance GetMostClosestWaypoint (GeoLocation center, IList<TollPoint> points);
        ParallelQuery<TollPoint> GetLocationsFromRadius (GeoLocation center, IList<TollPoint> points);
        TollPoint GetLocationFromRadius (GeoLocation center, IList<TollPoint> points);
	}
}

