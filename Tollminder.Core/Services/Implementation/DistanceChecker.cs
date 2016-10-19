using System;
using System.Linq;
using Tollminder.Core.Models;
using System.Collections.Generic; 
using Tollminder.Core.Helpers;
using System.Threading.Tasks;
using static Tollminder.Core.Helpers.LocationChecker;

namespace Tollminder.Core.Services.Implementation
{
	public class DistanceChecker : IDistanceChecker
	{
		public double Epsilon { get; } = double.Epsilon;

		private readonly IGeoLocationWatcher _geoWatcher;
		private readonly IWaypointChecker _waypointChecker;

		public DistanceChecker (IGeoLocationWatcher geoWatcher, IWaypointChecker waypointChecker)
		{
			this._geoWatcher = geoWatcher;
			this._waypointChecker = waypointChecker;
		}

        public List<TollPointWithDistance> GetMostClosestWaypoint (GeoLocation center, List<TollPoint> points, double radius = double.MaxValue)
		{
            return points.AsParallel()
                         .AsOrdered()
                         .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                         .Select(x => new TollPointWithDistance(x, DistanceBetweenGeoLocations(center, x.Location)))
                         .OrderBy(x => x.Distance)
                         .Where(x => x.Distance - radius < Epsilon)
                         .ToList();
		}
    }
}