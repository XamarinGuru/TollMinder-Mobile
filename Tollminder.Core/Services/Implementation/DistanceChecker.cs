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
		public static double DistanceToWaypoint { get; } = 0.6;
		public double Epsilon { get; } = 0;
		public double WaypointDistanceRequired { get; } = 0.02;

		private readonly IGeoLocationWatcher _geoWatcher;
		private readonly IWaypointChecker _waypointChecker;

		public DistanceChecker (IGeoLocationWatcher geoWatcher, IWaypointChecker waypointChecker)
		{
			this._geoWatcher = geoWatcher;
			this._waypointChecker = waypointChecker;
		}

		public virtual TollRoadWaypointWithDistance GetMostClosestWaypoint (GeoLocation center, IList<TollRoadWaypoint> points)
		{
			var point = points.AsParallel ().WithMergeOptions (ParallelMergeOptions.AutoBuffered).OrderBy (x =>
			{
				var a = DistanceBetweenGeoLocations(center, x.Location);
				System.Diagnostics.Debug.WriteLine($"{a:0.##} {x.Name} {x.Location}");
				return a;
			}).FirstOrDefault ();

			if (point != null)
			{
				var tollRoadWaypointWithDistance = new TollRoadWaypointWithDistance(point);
				tollRoadWaypointWithDistance.Distance = DistanceBetweenGeoLocations(center, point.Location);
				return tollRoadWaypointWithDistance;
			}

			return null;
		}

		public virtual ParallelQuery<TollRoadWaypoint> GetLocationsFromRadius (GeoLocation center, IList<TollRoadWaypoint> points)
		{
			return points.AsParallel ().WithMergeOptions (ParallelMergeOptions.AutoBuffered).Where (x => (DistanceBetweenGeoLocations (center, x.Location) - DistanceToWaypoint) < Epsilon);
		}

		public virtual Task<ParallelQuery<TollRoadWaypoint>> GetLocationsFromRadiusAsync (GeoLocation center, IList<TollRoadWaypoint> points)
		{
			return Task.Run(() => {
				return points.AsParallel().WithMergeOptions(ParallelMergeOptions.AutoBuffered).Where (x => (DistanceBetweenGeoLocations (center, x.Location) - DistanceToWaypoint) < Epsilon);
			});
		}

		public virtual TollRoadWaypoint GetLocationFromRadius (GeoLocation center, IList<TollRoadWaypoint> points)
		{
			return points.AsParallel ().WithMergeOptions (ParallelMergeOptions.AutoBuffered).FirstOrDefault (x => (DistanceBetweenGeoLocations (center, x.Location) - DistanceToWaypoint) < Epsilon);
		}

		public virtual Task<TollRoadWaypoint> GetLocationFromRadiusAsync (GeoLocation center, IList<TollRoadWaypoint> points)
		{
			return Task.Run (() => {
				var point = points.AsParallel ().WithMergeOptions (ParallelMergeOptions.AutoBuffered).FirstOrDefault (x => 
					{
						Log.LogMessage (string.Format ("{0} - {1} = {2}", DistanceBetweenGeoLocations (center, x.Location), DistanceToWaypoint,  DistanceBetweenGeoLocations (center, x.Location) - DistanceToWaypoint));

						return DistanceBetweenGeoLocations (center, x.Location) - DistanceToWaypoint <= Epsilon;
					});
				return point;
			});
		}
	}
}