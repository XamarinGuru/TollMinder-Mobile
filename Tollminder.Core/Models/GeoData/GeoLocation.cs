using System;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.Models
{
    // encapulates a geolocation and other information
    public class GeoLocation : IEquatable<GeoLocation>
    {
        public const double DesiredAccuracy = 100;
        const double Epsilon = 0.0001;

        public string TollPointId { get; set; }
        public double Speed { get; set; }
        public double Latitude { get; set; }

        private double longitude;
        public double Longitude
        {
            get
            {
                double newLongitude = longitude == SettingsService.wrongLongitude ? SettingsService.longitude : longitude;
                return newLongitude;
            }
            set
            {
                longitude = value;
                if (longitude != value)
                {
                    SettingsService.longitude = value;
                    SettingsService.wrongLongitude = longitude;
                }
            }
        }

        public double Accuracy { get; set; }
        public double Altitude { get; set; }
        public double AltitudeAccuracy { get; set; }

        // are we at the desired accuracy yet?
        public bool IsDesiredAccuracy
        {
            get { return Accuracy < DesiredAccuracy; }
        }

        public bool IsUnknownGeoLocation
        {
            get { return Math.Abs(Latitude) < Epsilon && Math.Abs(Longitude) < Epsilon; }
        }

        public GeoLocation()
        {
        }

        public GeoLocation(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }

        // does this equal another location?
        public bool Equals(GeoLocation other)
        {
            var result = ((Math.Abs(Latitude - other.Latitude) < Epsilon)
                && (Math.Abs(Longitude - other.Longitude) < Epsilon)
                && (Math.Abs(Altitude - other.Altitude) < Epsilon));
            return result;
        }
    }
}

