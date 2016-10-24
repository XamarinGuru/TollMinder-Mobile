using System.Linq;
using Tollminder.Core.Models;
using System.Collections.Generic; 
using static Tollminder.Core.Helpers.LocationChecker;

namespace Tollminder.Core.Services.Implementation
{
	public class DistanceChecker : IDistanceChecker
	{
		public double Epsilon { get; } = double.Epsilon;

        public List<TollPointWithDistance> GetMostClosestWaypoint (GeoLocation center, List<TollPoint> points, double radius = double.MaxValue)
		{
            return points.AsParallel()
                         .AsOrdered()
                         .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                         .Where(x => DistanceBetweenGeoLocations(center, x.Location) - radius < Epsilon)
                         .Select(x => new TollPointWithDistance(x, DistanceBetweenGeoLocations(center, x.Location)))
                         .OrderBy(x => x.Distance)
                         .ToList();
		}
    }
}