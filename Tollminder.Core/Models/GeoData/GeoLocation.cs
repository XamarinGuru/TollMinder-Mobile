﻿using System;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Chance.MvvmCross.Plugins.UserInteraction;
using Xamarin;
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

        public GeoLocation(string location)
        {
            try
            {
                var coords = location.Split(',');
                Latitude = double.Parse(coords[0]);
                Longitude = double.Parse(coords[1]);
            }
            catch (Exception ex)
            {
                Log.LogMessage($"Wrong dummy location: {location}, ex {ex.Message + ex.StackTrace}");
                Mvx.Resolve<IUserInteraction>().Alert("Wrong location data", null, "Error", "Ok");
                Insights.Report(ex);
            }
        }

        // does this equal another location?
        public bool Equals(GeoLocation other)
        {
            return ((Math.Abs(Latitude - other.Latitude) < Epsilon)
                && (Math.Abs(Longitude - other.Longitude) < Epsilon)
                && (Math.Abs(Altitude - other.Altitude) < Epsilon));
        }

        public override string ToString()
        {
            if (SettingsService.wrongLongitude == Longitude && Longitude != 0)
            {
                Insights.Report(new Exception("This shit again change minus on plus!!!"));
                Mvx.Resolve<IUserInteraction>().Alert("You received a wrong longitude!", null, "Error", "Ok");
            }
            return string.Format("{0}, {1}", Latitude, Longitude);
        }
    }
}

