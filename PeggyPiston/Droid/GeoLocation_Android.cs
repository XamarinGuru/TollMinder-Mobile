/*
 * basically ripped from here:
 * http://developer.xamarin.com/guides/android/platform_features/maps_and_location/location/
 * 
 * and here:
 * https://github.com/raechten/TestGPS
 * 
*/

using Android.Locations;
using Xamarin.Forms;
using Android.Content;
using PeggyPiston.Droid;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


[assembly: Dependency (typeof (GeoLocation_Android))]

namespace PeggyPiston.Droid
{
	public class GeoLocation_Android : Java.Lang.Object, IGeoLocation, ILocationListener
	{
		LocationManager _locationManager;
		Location _currentLocation { get; set; }

		public GeoLocation_Android()
		{
			InitializeLocationManager();
		}

		void InitializeLocationManager()
		{
			_locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);


			// doing it this way works.
			if (_locationManager.AllProviders.Contains (LocationManager.NetworkProvider)
				&& _locationManager.IsProviderEnabled (LocationManager.NetworkProvider)) {
				_locationManager.RequestLocationUpdates (LocationManager.NetworkProvider, 2000, 1, this);

			} else {
				System.Diagnostics.Debug.WriteLine ("LocationManager.NetworkProvider not available - not requesting updates");
			}

		}

		//ILocationService methods
		public void SetLocation()
		{
			System.Diagnostics.Debug.WriteLine ("calling setlocation -- we don't really ever do this.");

		}

		// ILocationListener methods
		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			if (_currentLocation == null)
			{
				System.Diagnostics.Debug.WriteLine ("location could not be determined");
				MessagingCenter.Send<IGeoLocation, string> (this, "LocationUnavailable", "Unable to determine your location.");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine ("location was changed");

				new Thread (new ThreadStart (() => {
				
					var geocoder = new Geocoder(Forms.Context);
					IList<Address> addressList = geocoder.GetFromLocation(_currentLocation.Latitude, _currentLocation.Longitude, 10);

					Address address = addressList.FirstOrDefault();
					if (address != null)
					{
						var deviceAddress = new StringBuilder();
						for (int i = 0; i < address.MaxAddressLineIndex; i++)
						{
							deviceAddress.Append(address.GetAddressLine(i)).AppendLine(",");
						}

						MessagingCenter.Send<IGeoLocation, string> (this, "TestingLocation", deviceAddress.ToString());
					}
					else
					{
						MessagingCenter.Send<IGeoLocation, string> (this, "TestingLocation", "Unable to determine the address.");
					}				
				
				
				} )).Start ();

			}
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




