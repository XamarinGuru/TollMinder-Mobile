using System;
using CoreLocation;
using Tollminder.Core.Models;

namespace Tollminder.Touch.Helpers
{
	public static class Extensions
	{
		public static GeoLocation GetGeoLocationFromCLLocation (this CLLocation loc)
		{
			var geoLocation = new GeoLocation()
			{
				Speed = loc.Speed,
				Longitude = loc.Coordinate.Longitude,
				Latitude = loc.Coordinate.Latitude,
				Altitude = loc.Altitude
			};
			return geoLocation;
		}
	}
}

