using System;
using Tollminder.Core.Models;
using Android.Locations;
using Android.Widget;
using Android.OS;
using Android.Gms.Location;
using Tollminder.Core.Helpers;
using Android.Content;
using Android.Gms.Common;

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

        public static GeoLocation GetGeolocationFromAndroidLocation(this Location loc)
        {
            Log.LogMessage($"Recieved new location from broadcast receiver {loc}");
            var geoLocation = new GeoLocation();
            Log.LogMessage(string.Format("ACCURACY IS {0}", loc.Accuracy));
            geoLocation.Accuracy = loc.Accuracy;
            geoLocation.Altitude = loc.Altitude;
            geoLocation.Longitude = loc.Longitude;//-74.295085;//
            geoLocation.Latitude = loc.Latitude;
            geoLocation.Speed = loc.Speed;
            return geoLocation;
        }

        public static GeoLocation GetGeolocationFromAndroidLocation(this Bundle bundle)
        {
            var geoLocation = new GeoLocation();
            geoLocation.Accuracy = bundle.GetDouble(Accuracy);
            geoLocation.Altitude = bundle.GetDouble(Altitude);
            geoLocation.Longitude = bundle.GetDouble(Longitude);
            geoLocation.Latitude = bundle.GetDouble(Latitude);
            geoLocation.Speed = bundle.GetDouble(SpeedKey);
            return geoLocation;

        }

        public static Bundle GetBundleFromLocation(this GeoLocation loc)
        {
            var bundle = new Bundle();
            bundle.PutDouble(Accuracy, loc.Accuracy);
            bundle.PutDouble(Altitude, loc.Altitude);
            bundle.PutDouble(Longitude, loc.Longitude);
            bundle.PutDouble(Latitude, loc.Latitude);
            bundle.PutDouble(SpeedKey, loc.Speed);
            return bundle;
        }

        public static MotionType GetMotionType(this DetectedActivity detectedAcitvity)
        {
            switch (detectedAcitvity.Type)
            {
                case DetectedActivity.InVehicle:
                    return MotionType.Automotive;
                case DetectedActivity.OnBicycle:
                    return MotionType.Automotive;
                case DetectedActivity.OnFoot:
                    return MotionType.Walking;
                case DetectedActivity.Running:
                    return MotionType.Running;
                case DetectedActivity.Still:
                    return MotionType.Still;
                case DetectedActivity.Tilting:
                    return MotionType.Unknown;
                case DetectedActivity.Walking:
                    return MotionType.Walking;
                default:
                    return MotionType.Unknown;
            }
        }

        public static Bundle PutMotionType(this MotionType mType)
        {
            var bundle = new Bundle();
            bundle.PutInt(MotionTypeKey, (int)mType);
            return bundle;
        }

        public static MotionType GetMotionType(this Bundle mType)
        {
            switch (mType.GetInt(MotionTypeKey))
            {
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

        public static Bundle GetBundle(this bool mType)
        {
            var bundle = new Bundle();
            bundle.PutBoolean(IsEnabled, mType);
            return bundle;
        }

        public static bool GetIsEnabled(this Bundle mType)
        {
            return mType.GetBoolean(IsEnabled);
        }

        public static bool IsGooglePlayServicesInstalled(this Context context)
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
            if (queryResult == ConnectionResult.Success)
            {
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
            }
            return false;
        }
    }
}

