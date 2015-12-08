using System;
using Tollminder.Core.Models;
using Android.Locations;
using Android.Widget;

namespace Tollminder.Droid
{
	public static class Etensions
	{
		public static GeoLocation GetGeolocationFromAndroidLocation (this Location loc)
		{
			var geoLocation = new GeoLocation (); 
			geoLocation.Accuracy = loc.Accuracy;
			geoLocation.Altitude = loc.Altitude;
			geoLocation.Longitude = loc.Longitude;
			geoLocation.Latitude = loc.Latitude;
			geoLocation.Speed = loc.Speed;
			return geoLocation;
		}
	}
}

