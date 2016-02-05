﻿using System;
using Tollminder.Core.Models;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


namespace Tollminder.Core.Helpers
{
	public static class LocationChecker
	{		
		public static double ToRadians(this double val)
		{
			return (180 / Math.PI) * val;
		}

		public static double ToDegrees (this double val)
		{
			return (180 / Math.PI) / val;
		}

		// cos(d) = sin(φА)·sin(φB) + cos(φА)·cos(φB)·cos(λА − λB),
		//  where φА, φB are latitudes and λА, λB are longitudes
		// Distance = d * R
		public static double DistanceBetweenGeoLocations (GeoLocation center, GeoLocation otherPoint)
		{
			
			double R = 6371 / 4; // 250 metres

			double sLat1 = Math.Sin(center.Latitude.ToRadians());
			double sLat2 = Math.Sin(otherPoint.Latitude.ToRadians());
			double cLat1 = Math.Cos(center.Latitude.ToRadians());
			double cLat2 = Math.Cos(otherPoint.Latitude.ToRadians());
			double cLon = Math.Cos(center.Longitude.ToRadians() - otherPoint.Longitude.ToRadians());

			double cosD = sLat1*sLat2 + cLat1*cLat2*cLon;

			double d = Math.Acos(cosD);

			double dist = R * d;

			return dist;

		}

		public static ParallelQuery<TollRoadWaypoint> GetLocationsFromRadius (this GeoLocation center, IList<TollRoadWaypoint> points)
		{
			return points.AsParallel().WithMergeOptions(ParallelMergeOptions.AutoBuffered).Where (x => DistanceBetweenGeoLocations (center, x.Location) <= 200);
		}

		public static Task<ParallelQuery<TollRoadWaypoint>> GetLocationsFromRadiusAsync (this GeoLocation center, IList<TollRoadWaypoint> points)
		{
			return Task.Run(() => {
				return points.AsParallel().WithMergeOptions(ParallelMergeOptions.AutoBuffered).Where (x => DistanceBetweenGeoLocations (center, x.Location) <= 200);
			});
		}

		public static TollRoadWaypoint GetLocationFromRadius (this GeoLocation center, IList<TollRoadWaypoint> points)
		{
			return points.AsParallel ().WithMergeOptions (ParallelMergeOptions.AutoBuffered).FirstOrDefault (x => DistanceBetweenGeoLocations (center, x.Location) <= 200);
		}

		public static Task<TollRoadWaypoint> GetLocationFromRadiusAsync (this GeoLocation center, IList<TollRoadWaypoint> points)
		{
			return Task.Run(() => {
				var point = points.AsParallel ().WithMergeOptions (ParallelMergeOptions.AutoBuffered).FirstOrDefault (x => DistanceBetweenGeoLocations (center, x.Location) <= 200);
				return point;
			});
		}

	}
}

