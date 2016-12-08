using System;
using System.Diagnostics;
using Tollminder.Core.Models;

namespace Tollminder.Core.Helpers
{
	public static class LocationChecker
	{		
		public static double ToRadians(this double val)
		{
			return (Math.PI / 180) * val;
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
			const double R = 6371;

			double sLat1 = Math.Sin(center.Latitude.ToRadians());
			double sLat2 = Math.Sin(otherPoint.Latitude.ToRadians());
			double cLat1 = Math.Cos(center.Latitude.ToRadians());
			double cLat2 = Math.Cos(otherPoint.Latitude.ToRadians());
			double cLon = Math.Cos(center.Longitude.ToRadians() - otherPoint.Longitude.ToRadians());

			double cosD = sLat1*sLat2 + cLat1*cLat2*cLon;
            Debug.WriteLine(cosD);

			double d = Math.Acos(cosD);
            Debug.WriteLine(d);
			
            double dist = R * d;
            Debug.WriteLine(dist);
			
            return dist;
		}
	}
}