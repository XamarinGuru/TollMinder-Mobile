using System;
using Tollminder.Core.Models;
using Android.Locations;
using Android.Widget;
using Android.OS;
using Android.Gms.Fitness.Data;
using Android.Gms.Location;

namespace Tollminder.Droid
{
	public static class Extensions
	{
		public const string Accuracy = "Accuracy";
		public const string Altitude = "Altitude";
		public const string Longitude = "Longitude";
		public const string Latitude = "Latitude";
		public const string SpeedKey = "Speed";
		public const string MotionTypeKey = "MotionType";
		public const string IsEnabled = "IsEnabled";

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
			geoLocation.Speed = bundle.GetDouble (SpeedKey);
			return geoLocation;

		}

		public static Bundle GetGeolocationFromAndroidLocation (this GeoLocation loc)
		{
			var bundle = new Bundle ();
			bundle.PutDouble (Accuracy, loc.Accuracy);
			bundle.PutDouble (Altitude, loc.Altitude);
			bundle.PutDouble (Longitude, loc.Longitude);
			bundle.PutDouble (Latitude, loc.Latitude);
			bundle.PutDouble (SpeedKey, loc.Speed);
			return bundle;	
		}

		public static MotionType GetMotionType (this DetectedActivity detectedAcitvity)
		{
			switch (detectedAcitvity.Type) {
			case 3:
				return MotionType.Still;
			case 7:
				return MotionType.Walking;
			case 8:
				return MotionType.Running;
			case 0:
				return MotionType.Automotive;
			case 4: 
				return MotionType.Unknown;
			}
			return MotionType.Unknown;
		}

		public static Bundle PutMotionType (this MotionType mType)
		{
			var bundle = new Bundle ();
			bundle.PutInt (MotionTypeKey, (int)mType);
			return bundle;	
		}

		public static MotionType GetMotionType (this Bundle mType)
		{			
			switch (mType.GetInt(MotionTypeKey)) {
			case (int)MotionType.Still:
				return MotionType.Still;
			case (int)MotionType.Walking:
				return MotionType.Walking;
			case (int)MotionType.Running:
				return MotionType.Running;
			case (int)MotionType.Automotive:
				return MotionType.Automotive;
			default:
				return MotionType.Unknown;
			}
		}

		public static Bundle GetBundle (this bool mType) 
		{
			var bundle = new Bundle ();
			bundle.PutBoolean (IsEnabled, mType);
			return bundle;	
		}

		public static bool GetIsEnabled (this Bundle mType) 
		{			
			return mType.GetBoolean(IsEnabled);	
		}
	}
}

