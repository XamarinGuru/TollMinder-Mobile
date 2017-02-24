using System.Linq;
using Tollminder.Core.Models;
using System.Collections.Generic; 
using static Tollminder.Core.Helpers.LocationChecker;

namespace Tollminder.Core.Services.Implementation
{
	public class DistanceChecker : IDistanceChecker
	{
		public double Epsilon { get; } = double.Epsilon;

        public List<TollPointWithDistance> GetMostClosestTollPoint (GeoLocation center, IList<TollPoint> points, double radius = double.MaxValue)
		{
            return points.AsParallel()
                         .AsOrdered()
                         .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                         .Where(x => DistanceBetweenGeoLocations(center, new GeoLocation { Longitude = x.Longitude, Latitude = x.Latitude }) - radius < Epsilon)
                         .Select(x => new TollPointWithDistance(x, DistanceBetweenGeoLocations(center, new GeoLocation{Longitude = x.Longitude, Latitude = x.Latitude})))
                         .OrderBy(x => x.Distance)
                         .ToList();
		}
    }
}