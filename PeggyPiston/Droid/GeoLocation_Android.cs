using System;
using PeggyPiston.Droid;
using Xamarin.Forms;
using System.Threading.Tasks;
using Android.Util;
using Android.Content;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Dependency (typeof (GeoLocation_Android))]

namespace PeggyPiston.Droid
{
	public class GeoLocation_Android : IGeoLocation
	{
		protected readonly string logTag = "GeoLocation_Android";

		private Location _currentLocation;
		protected LocationServiceConnection locationServiceConnection;

		public LocationService LocationService
		{
			get {
				if (locationServiceConnection.Binder == null) {
					throw new Exception ("Service not bound yet");
				}

				// note that we use the ServiceConnection to get the Binder, and the Binder to get the Service here
				return locationServiceConnection.Binder.Service;
			}
		}

		#region IGeoLocation implementation

		public void SetLocation ()
		{
			Log.Debug (logTag, "Calling SetLocation");
		}

		#endregion

		public GeoLocation_Android()
		{
			InitializeLocationManager();
		}

		void InitializeLocationManager()
		{
			// starting a service like this is blocking, so we want to do it on a background thread
			new Task ( () => { 

				// start our main service
				Log.Debug (logTag, "Calling StartService");
				Android.App.Application.Context.StartService (new Intent (Android.App.Application.Context, typeof(LocationService)));

				// create a new service connection so we can get a binder to the service
				locationServiceConnection = new LocationServiceConnection (null);

				// this event will fire when the Service connection in the OnServiceConnected call 
				locationServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => {

					Log.Debug (logTag, "Service Connected");
					// we will use this event to notify the forms app when to start updating the UI
					MessagingCenter.Send<IGeoLocation, string> (this, "Debug", "Location service task successful.");

					LocationService.LocationChanged += HandleLocationChanged;

				};

				// bind our service (Android goes and finds the running service by type, and puts a reference
				// on the binder to that service)
				// The Intent tells the OS where to find our Service (the Context) and the Type of Service
				// we're looking for (LocationService)
				Intent locationServiceIntent = new Intent (Android.App.Application.Context, typeof(LocationService));
				Log.Debug (logTag, "Calling service binding");

				// Finally, we can bind to the Service using our Intent and the ServiceConnection we
				// created in a previous step.
				Android.App.Application.Context.BindService (locationServiceIntent, locationServiceConnection, Bind.AutoCreate);

			} ).Start ();

		}

		public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			Android.Locations.Location location = e.Location;
			Log.Debug (logTag, "Foreground updating");

			_currentLocation = location;
			if (_currentLocation == null)
			{
				System.Diagnostics.Debug.WriteLine ("location could not be determined");
				MessagingCenter.Send<IGeoLocation, string> (this, "LocationUnavailable", "Unable to determine your location.");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine ("location was changed");

				Log.Debug ("LocationService", String.Format ("Latitude is {0}", _currentLocation.Latitude));
				Log.Debug ("LocationService", String.Format ("Longitude is {0}", _currentLocation.Longitude));
				Log.Debug ("LocationService", String.Format ("Altitude is {0}", _currentLocation.Altitude));
				Log.Debug ("LocationService", String.Format ("Speed is {0}", _currentLocation.Speed));
				Log.Debug ("LocationService", String.Format ("Accuracy is {0}", _currentLocation.Accuracy));
				Log.Debug ("LocationService", String.Format ("Bearing is {0}", _currentLocation.Bearing));

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

					MessagingCenter.Send<IGeoLocation, string> (this, "LocationService", deviceAddress.ToString());
				}
				else
				{
					MessagingCenter.Send<IGeoLocation, string> (this, "LocationService", "Unable to determine the address.");
				}				


			}
		}
	}
}




