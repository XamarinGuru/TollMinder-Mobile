/*
 * basically ripped from here:
 * http://developer.xamarin.com/guides/android/platform_features/maps_and_location/location/
 * 
 * and here:
 * https://github.com/raechten/TestGPS
 * 
*/

using System;
using Android.Locations;
using Xamarin.Forms;
using Android.Content;
using PeggyPiston.Droid;
using System.Collections.Generic;
using System.Linq;


[assembly: Dependency (typeof (GeoLocation_Android))]

namespace PeggyPiston.Droid
{
	public class GeoLocation_Android : Java.Lang.Object, IGeoLocation, ILocationListener
	{
		private LocationManager _locationManager;
		private string _locationProvider;
		private Location _currentLocation { get; set; }

		public GeoLocation_Android()
		{
			this.InitializeLocationManager();
		}

		void InitializeLocationManager()
		{
			_locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);


			// doing it this way works.
			if (_locationManager.AllProviders.Contains (LocationManager.NetworkProvider)
				&& _locationManager.IsProviderEnabled (LocationManager.NetworkProvider)) {
				_locationManager.RequestLocationUpdates (LocationManager.NetworkProvider, 2000, 1, this);
			} else {
				System.Diagnostics.Debug.WriteLine ("location provider not available - not requesting updates");
			}


			// doing it this way, does not.
			/*
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Coarse
			};
			IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

			if (acceptableLocationProviders.Any())
			{
				_locationProvider = acceptableLocationProviders.First();
			}
			else
			{
				_locationProvider = String.Empty;
			}

			//MessagingCenter.Subscribe<IGeoLocation,string>(this, "init state string", HandleLocationUpdate);
*/

		}

		//ILocationService methods
		public void Start()
		{

			if (!string.IsNullOrEmpty(_locationProvider) && _locationManager.IsProviderEnabled(_locationProvider))
			{
				_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
				System.Diagnostics.Debug.WriteLine ("requesting location updates");

			}
			else
			{
				// Do something here to notify the user we can't get the location updates set up properly
				System.Diagnostics.Debug.WriteLine("No (enabled) location provider available.");
			}

		}

		public void SetLocation()
		{
			var currentLocationString = _currentLocation == null 
				? "Can't determine the current address." 
				: string.Format("{0} - {1}", _currentLocation.Latitude, _currentLocation.Longitude);

			MessagingCenter.Send<IGeoLocation, string> (this, "TestingLocation", currentLocationString);

		}

		// ILocationListener methods
		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			if (_currentLocation == null)
			{
				System.Diagnostics.Debug.WriteLine ("location could not be determined");
				MessagingCenter.Send<IGeoLocation, string> (this, "TestingLocation", "Unable to determine your location.");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine ("location was changed");
				MessagingCenter.Send<IGeoLocation, string> (this, "TestingLocation", String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude));
			}
			//SetLocation();
		}

		public void OnStatusChanged(string provider, Availability status, global::Android.OS.Bundle extras)
		{
			//Not Implemented
			System.Diagnostics.Debug.WriteLine ("status changed");
		}

		public void OnProviderDisabled(string provider)
		{
			//Not Implemented
			System.Diagnostics.Debug.WriteLine ("provider disabled");
		}

		public void OnProviderEnabled(string provider)
		{
			//Not Implemented
			System.Diagnostics.Debug.WriteLine ("provider enabled");
		}
	}
}




