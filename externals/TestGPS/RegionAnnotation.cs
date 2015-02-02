using System;
using MonoTouch.Foundation;
using MonoTouch.MapKit;
using MonoTouch.CoreLocation;

namespace TestGPS
{
	public  class   RegionAnnotation:MKAnnotation
	{


		public CLRegion Region{ get; set;}
		//public static CLLocationDistance Radius;

		public double Radius{ get; set;}
		public override CLLocationCoordinate2D Coordinate
		{

			get;

			set;
		}
	
		public override string Subtitle
		{

			get
			{
				return "Mittu";
			}
		}

		public override string Title
		{

			get
			{
				return "Mohit";
			}
		}
	
		public CLLocationCoordinate2D coordinate{ get; set;}
		public RegionAnnotation ()
		{
		}
		public RegionAnnotation (CLRegion newRegion)
		{
			this.Region = newRegion;
			this.Coordinate = newRegion.Center;
			this.Radius = newRegion.Radius;

		}

		public void setRadius(double newRadius) {
			//[self willChangeValueForKey:@"subtitle"];
			this.WillChangeValue("subtitle");
			this.Radius = newRadius;
			this.DidChangeValue ("subtitle");
			//[self didChangeValueForKey:@"subtitle"];
		}


		public string subtitle() {
			//return [NSString stringWithFormat: @"Lat: %.4F, Lon: %.4F, Rad: %.1fm", coordinate.latitude, coordinate.longitude, radius];
			return String.Format ("Lat: %.4F, Lon: %.4F, Rad: %.1fm",Coordinate.Latitude, Coordinate.Longitude, Radius);
		}

	}
}

