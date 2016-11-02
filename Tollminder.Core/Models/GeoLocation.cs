using System;
using MvvmCross.Platform;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models
{
	// encapulates a geolocation and other information
	public class GeoLocation : IEquatable<GeoLocation>, IDatabaseEntry
	{
        public const double DesiredAccuracy = 100;
		const double Epsilon = 0.0001;

        [PrimaryKey, AutoIncrement]
        public long DBId { get; set; }
        [ForeignKey(typeof(TollPoint))]
        public long TollPointId { get; set; }
		public double Speed { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
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
			get { return Math.Abs (Latitude) < Epsilon && Math.Abs (Longitude) < Epsilon; }
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
                Latitude = double.Parse(coords[0], System.Globalization.CultureInfo.InvariantCulture);
                Longitude = double.Parse(coords[1], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Log.LogMessage($"Wrong dummy location: {location}, ex {ex.Message + ex.StackTrace}");
                throw new Exception("Wrong location data");
            }
        }

		// does this equal another location?
		public bool Equals(GeoLocation other)
		{
			return ((Math.Abs (Latitude - other.Latitude) < Epsilon)
				&& (Math.Abs (Longitude - other.Longitude) < Epsilon)
				&& (Math.Abs (Altitude - other.Altitude) < Epsilon));
		}

		public override string ToString()
		{
			return string.Format("{0},{1}", Latitude, Longitude);
		}
	}
}

