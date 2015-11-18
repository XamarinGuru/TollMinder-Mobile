using System;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;


using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using MonoTouch.UIKit;

namespace TestGPS
{
	public class LocationManager:CLLocationManagerDelegate
	{
		CLLocationManager locationManager{ get; set;}	
		public LocationManager ()
		{
			this.locationManager = new CLLocationManager ();
		}
		public void callLocationService(){


			this.locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			this.locationManager.Delegate = this;
		
			this.locationManager.StartUpdatingLocation ();

		}
		public override void Failed (CLLocationManager manager, NSError error)
		{
			Console.WriteLine ("errorrrrrr"+error.LocalizedDescription);
		}
		public override void UpdatedLocation (CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
		{
			Console.WriteLine ("UpdatedHeading======{0}......{0}",newLocation,oldLocation);

			if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active) {

			} else {
				Console.WriteLine ("new location====={0}",newLocation);
				//NSLog(@"App is backgrounded. New location is %@", newLocation);
			}
		}

	}
}

