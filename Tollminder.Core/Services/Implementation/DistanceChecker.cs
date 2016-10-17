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

		public virtual TollPointWithDistance GetMostClosestWaypoint (GeoLocation center, IList<TollPoint> points)
		{
			var pts = points.AsParallel().AsOrdered().WithMergeOptions(ParallelMergeOptions.FullyBuffered).OrderBy(x => DistanceBetweenGeoLocations(center, x.Location));

			foreach(var x in pts)
			{
				var distance = DistanceBetweenGeoLocations(center, x.Location);
				Log.LogMessage($"{distance:0.##} {x.Name} {x.Location}");
			}

			TollPoint point =  pts?.FirstOrDefault ();

			if (point != null)
			{
				var tollRoadWaypointWithDistance = new TollPointWithDistance(point);
				tollRoadWaypointWithDistance.Distance = DistanceBetweenGeoLocations(center, point.Location);
				return tollRoadWaypointWithDistance;
			}

			return null;
		}

        public virtual ParallelQuery<TollPoint> GetLocationsFromRadius (GeoLocation center, IList<TollPoint> points)
		{
			return points.AsParallel ().WithMergeOptions (ParallelMergeOptions.AutoBuffered).Where (x => (DistanceBetweenGeoLocations (center, x.Location) - SettingsService.WaypointLargeRadius) < Epsilon);
		}

		public virtual TollPoint GetLocationFromRadius (GeoLocation center, IList<TollPoint> points)
		{
			return points.AsParallel ().WithMergeOptions (ParallelMergeOptions.AutoBuffered).FirstOrDefault (x => (DistanceBetweenGeoLocations (center, x.Location) - SettingsService.WaypointLargeRadius) < Epsilon);
		}
    }
}