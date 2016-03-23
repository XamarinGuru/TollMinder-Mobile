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
		public double DistanceToWaypoint { get; } = 0.6;
		public double Epsilon { get; } = 0;
		public double WaypointDistanceRequired { get; } = 0.02;

		private readonly IGeoLocationWatcher _geoWatcher;
		private readonly IWaypointChecker _waypointChecker;

		public DistanceChecker (IGeoLocationWatcher geoWatcher, IWaypointChecker waypointChecker)
		{
			this._geoWatcher = geoWatcher;
			this._waypointChecker = waypointChecker;
		}

		public double Distance { get; set; }

		public virtual bool IsCloserToWaypoint 
		{
			get {
				#if DEBUG
				Log.LogMessage (string.Format ("DISTANCE {0} {1}", DistanceBetweenGeoLocations (_geoWatcher.Location, _waypointChecker.Waypoint.Location), Distance));
				#endif
				return Distance - DistanceBetweenGeoLocations (_geoWatcher.Location, _waypointChecker.Waypoint.Location) >= 0;
			}
		}

		public bool IsAtWaypoint 
		{
			get {
				#if DEBUG
				Log.LogMessage (string.Format ("DIS : {0}, DIST 2 {1} = {2}", Distance, WaypointDistanceRequired, Distance - WaypointDistanceRequired));
				#endif
				return (Distance - WaypointDistanceRequired) < 0;
			}
		}

		public virtual void UpdateDistance ()
		{
			Distance = DistanceBetweenGeoLocations (_geoWatcher.Location, _waypointChecker.Waypoint.Location);
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
						#if DEBUG 
						Log.LogMessage (string.Format ("{0} - {1} = {2}", DistanceBetweenGeoLocations (center, x.Location), DistanceToWaypoint,  DistanceBetweenGeoLocations (center, x.Location) - DistanceToWaypoint));
						#endif
						return DistanceBetweenGeoLocations (center, x.Location) - DistanceToWaypoint <= Epsilon;
					});
				return point;
			});
		}
	}
}