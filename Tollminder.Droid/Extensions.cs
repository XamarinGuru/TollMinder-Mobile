using System;
using Tollminder.Core.Models;
using Android.Locations;
using Android.Widget;
using Android.OS;

namespace Tollminder.Droid
{
	public static class Extensions
	{
		public const string Accuracy = "Accuracy";
		public const string Altitude = "Altitude";
		public const string Longitude = "Longitude";
		public const string Latitude = "Latitude";
		public const string Speed = "Speed";

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

		public static GeoLocation GetGeolocationFromAndroidLocation (this Bundle bundle)
		{
			var geoLocation = new GeoLocation ();
			geoLocation.Accuracy = bundle.GetDouble (Accuracy);
			geoLocation.Altitude = bundle.GetDouble (Altitude);
			geoLocation.Longitude = bundle.GetDouble (Longitude);
			geoLocation.Latitude = bundle.GetDouble (Latitude);
			geoLocation.Speed = bundle.GetDouble (Speed);
			return geoLocation;

		}

		public static Bundle GetGeolocationFromAndroidLocation (this GeoLocation loc)
		{
			var bundle = new Bundle ();
			bundle.PutDouble (Accuracy, loc.Accuracy);
			bundle.PutDouble (Altitude, loc.Altitude);
			bundle.PutDouble (Longitude, loc.Longitude);
			bundle.PutDouble (Latitude, loc.Latitude);
			bundle.PutDouble (Speed, loc.Speed);
			return bundle;	
		}
	}
}

