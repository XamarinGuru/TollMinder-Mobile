using System;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using MonoTouch.Foundation;


namespace TestGPS
{


	public class MyLocation{

		public double lat{ get; set;}
		public double lng{ get; set;}
		public double speed{ get; set;}
		public double coarse{ get; set;}
		public DateTime timeintrval{ get; set;}
		public Double altitude{ get; set;}
		public string  haccuracy{ get; set;}
		public string  vaccuracy{ get; set;}

		public MyLocation(){

		}

	}

	public class Setting:NSObject
	{
		public bool isAllow{ get; set;}
		public int timeTrack{ get; set;}
		public int accuracy{ get; set;}
		public MyLocation location{ get; set;}

		public Setting ()
		{

		}

	}
}

