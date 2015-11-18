using Xamarin.Forms;
using PeggyPiston.iOS;
using CoreLocation;
using UIKit;

[assembly: Dependency (typeof (GeoLocation_iOS))]

namespace PeggyPiston.iOS
{
	public class GeoLocation_iOS : IGeoLocation
	{

		protected readonly string logChannel = "GeoLocation_iOS";

		CLLocationManager iPhoneLocationManager = null;


		public GeoLocation_iOS ()
		{
			InitializeLocationManager();
		}

		#region IGeoLocation implementation

		public double GetCurrentLongitude ()
		{
			throw new System.NotImplementedException ();
		}

		public double GetCurrentLattitude ()
		{
			throw new System.NotImplementedException ();
		}

		public double GetCurrentSpeed ()
		{
			throw new System.NotImplementedException ();
		}

		public double GetCurrentBearing ()
		{
			throw new System.NotImplementedException ();
		}

		public double GetCurrentAccuracy ()
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		private void InitializeLocationManager() {
			// initialize our location manager and callback handler
			iPhoneLocationManager = new CLLocationManager ();

			// uncomment this if you want to use the delegate pattern:
			//locationDelegate = new LocationDelegate (mainScreen);
			//iPhoneLocationManager.Delegate = locationDelegate;

			// you can set the update threshold and accuracy if you want:
			//iPhoneLocationManager.DistanceFilter = 10; // move ten meters before updating
			//iPhoneLocationManager.HeadingFilter = 3; // move 3 degrees before updating

			// you can also set the desired accuracy:
			iPhoneLocationManager.DesiredAccuracy = 10; // in meters
			// you can also use presets, which simply evalute to a double value:
			//iPhoneLocationManager.DesiredAccuracy = CLLocation.AccuracyNearestTenMeters;

			// handle the updated location method and update the UI
			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				iPhoneLocationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
					UpdateLocation (e.Locations [e.Locations.Length - 1]);
				};
			} else {
				#pragma warning disable 618
				// this won't be called on iOS 6 (deprecated)
				iPhoneLocationManager.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) => {
					UpdateLocation (e.NewLocation);
				};
				#pragma warning restore 618
			}

			//iOS 8 requires you to manually request authorization now - Note the Info.plist file has a new key called requestWhenInUseAuthorization added to.
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				iPhoneLocationManager.RequestWhenInUseAuthorization();
			}

			/*
			// handle the updated heading method and update the UI
			iPhoneLocationManager.UpdatedHeading += (object sender, CLHeadingUpdatedEventArgs e) => {
				mainScreen.LblMagneticHeading.Text = e.NewHeading.MagneticHeading.ToString () + "º";
				mainScreen.LblTrueHeading.Text = e.NewHeading.TrueHeading.ToString () + "º";
			};
			*/

			// start updating our location, et. al.
			if (CLLocationManager.LocationServicesEnabled)
				iPhoneLocationManager.StartUpdatingLocation ();
			if (CLLocationManager.HeadingAvailable)
				iPhoneLocationManager.StartUpdatingHeading ();

		}

		public void UpdateLocation (CLLocation newLocation)
		{
			/*
			ms.LblAltitude.Text = newLocation.Altitude.ToString () + " meters";
			ms.LblLongitude.Text = newLocation.Coordinate.Longitude.ToString () + "º";
			ms.LblLatitude.Text = newLocation.Coordinate.Latitude.ToString () + "º";
			ms.LblCourse.Text = newLocation.Course.ToString () + "º"; // in degrees.  0 is north.
			ms.LblSpeed.Text = newLocation.Speed.ToString () + " meters/s";

			// get the distance from here to paris
			ms.LblDistanceToParis.Text = (newLocation.DistanceFrom(new CLLocation(48.857, 2.351)) / 1000).ToString() + " km";


			// just average the accuracy numbers.
			// newLocation.horizontalAccuracy -- in meters
			// newLocation.verticalAccuracy -- in meters


			*/

			string locationSummary = newLocation.Coordinate.Longitude.ToString () + ", " + newLocation.Coordinate.Latitude.ToString ();
			PeggyUtils.DebugLog("ios location updated: " + locationSummary, logChannel);
			MessagingCenter.Send<IGeoLocation, string> (this, PeggyConstants.channelLocationService, locationSummary);
		}


		// from implementor.
		public void SetLocation ()
		{
		}
	}
}

