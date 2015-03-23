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
		protected readonly string logChannel = "GeoLocation_Android";

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
		public double GetCurrentLongitude()
		{
			return _currentLocation.Longitude;
		}

		public double GetCurrentLattitude ()
		{
			return _currentLocation.Latitude;
		}

		public double GetCurrentSpeed ()
		{
			return _currentLocation.Speed;
		}

		public double GetCurrentBearing ()
		{
			return _currentLocation.Bearing;
		}

		public double GetCurrentAccuracy ()
		{
			return _currentLocation.Accuracy;
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
				PeggyUtils.DebugLog("Calling StartService", logChannel);
				Android.App.Application.Context.StartService (new Intent (Android.App.Application.Context, typeof(LocationService)));

				// create a new service connection so we can get a binder to the service
				locationServiceConnection = new LocationServiceConnection (null);

				// this event will fire when the Service connection in the OnServiceConnected call 
				locationServiceConnection.ServiceConnected += (object sender, ServiceConnectedEventArgs e) => {

					PeggyUtils.DebugLog("Service Connected", logChannel);
					// we will use this event to notify the forms app when to start updating the UI
					MessagingCenter.Send<IGeoLocation, string> (this, PeggyConstants.channelDebug, "Location service task successful.");

					LocationService.LocationChanged += HandleLocationChanged;
					LocationService.ProviderEnabled += HandleProviderEnabled;
					LocationService.ProviderDisabled += HandleProviderDisabled;
					LocationService.StatusChanged += HandleStatusChanged;

				};

				// bind our service (Android goes and finds the running service by type, and puts a reference
				// on the binder to that service)
				// The Intent tells the OS where to find our Service (the Context) and the Type of Service
				// we're looking for (LocationService)
				Intent locationServiceIntent = new Intent (Android.App.Application.Context, typeof(LocationService));
				PeggyUtils.DebugLog("Calling service binding", logChannel);

				// Finally, we can bind to the Service using our Intent and the ServiceConnection we
				// created in a previous step.
				Android.App.Application.Context.BindService (locationServiceIntent, locationServiceConnection, Bind.AutoCreate);

			} ).Start ();

		}

		public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			Android.Locations.Location location = e.Location;
			PeggyUtils.DebugLog("Foreground updating", logChannel);

			_currentLocation = location;
			if (_currentLocation == null)
			{
				PeggyUtils.DebugLog("location could not be determined", logChannel);
				MessagingCenter.Send<IGeoLocation, string> (this, PeggyConstants.channelLocationUnavailable, "We are unable to determine your location. We'll try again shortly.");
			}
			else
			{
				PeggyUtils.DebugLog("location was changed", logChannel);

				PeggyUtils.DebugLog (String.Format ("Latitude is {0}", _currentLocation.Latitude), logChannel);
				PeggyUtils.DebugLog (String.Format ("Longitude is {0}", _currentLocation.Longitude), logChannel);
				PeggyUtils.DebugLog (String.Format ("Altitude is {0}", _currentLocation.Altitude), logChannel);
				PeggyUtils.DebugLog (String.Format ("Speed is {0}", _currentLocation.Speed), logChannel);
				PeggyUtils.DebugLog (String.Format ("Accuracy is {0}", _currentLocation.Accuracy), logChannel);
				PeggyUtils.DebugLog (String.Format ("Bearing is {0}", _currentLocation.Bearing), logChannel); // also in degrees.  0 is north.

				MessagingCenter.Send<IGeoLocation, double> (this, PeggyConstants.channelLocationAccuracyReady, _currentLocation.Accuracy);

/*
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

					MessagingCenter.Send<IGeoLocation, string> (this, PeggyConstants.channelLocationService, deviceAddress.ToString());
				}
				else
				{
					MessagingCenter.Send<IGeoLocation, string> (this, PeggyConstants.channelLocationService, "Unable to determine the address.");
				}				
*/

			}

		}

		public void HandleProviderEnabled(object sender, LocationChangedEventArgs e) {
			PeggyUtils.DebugLog("HandleProviderEnabled", logChannel);
		}

		public void HandleProviderDisabled(object sender, LocationChangedEventArgs e) {
			PeggyUtils.DebugLog("HandleProviderDisabled", logChannel);
		}

		public void HandleStatusChanged(object sender, LocationChangedEventArgs e) {
			PeggyUtils.DebugLog("HandleStatusChanged", logChannel);
		}

	}
}




