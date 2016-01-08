using System;
using SQLite.Net.Attributes;

namespace Tollminder.Core.Models
{
	// encapulates a geolocation and other information
	public class GeoLocation : IEquatable<GeoLocation>
	{
		const double Epsilon = 0.0000001;

		public GeoLocation () 
		{
			
		}

		public GeoLocation (double lat, double lng)
		{
			Latitude = lat;
			Longitude = lng;
		}

		public const double DesiredAccuracy = 100;

		[PrimaryKey,AutoIncrement]
		public long Id { get; set; }
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

		// does this equal another location?
		public bool Equals(GeoLocation other)
		{
			return ((Latitude == other.Latitude)
				&& (Longitude == other.Longitude)
				&& (Altitude == other.Altitude));
		}

		public override string ToString()
		{
			return string.Format("{0},{1}", Latitude, Longitude);
		}
	}
}

