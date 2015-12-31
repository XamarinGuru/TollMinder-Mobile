using System;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Cirrious.CrossCore;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Droid.Helpers;
using Tollminder.Droid.Handlers;

namespace Tollminder.Droid.AndroidServices
{
	[Service]
	public class GeolocationService : GoogleApiService<GeolocationServiceHandler>,
										Android.Gms.Location.ILocationListener									
										 
	{
		const int NumberOfUpdates = 1;
		private LocationRequest _locationRequest;

		private GeoLocation _geoLocation;
		public virtual GeoLocation Location {
			get { return _geoLocation; } 
			protected set { 
				_geoLocation = value;
				SendMessage ();
			}
		}


		public override void OnCreate ()
		{
			base.OnCreate ();
			CreateGoogleApiClient (LocationServices.API);
			Connect ();
		}

		public override void OnConnected (Bundle connectionHint)
		{
			base.OnConnected (connectionHint);
			GetLocation ();
			#if DEBUG
			Log.LogMessage("GoogleApiClient connected");
			#endif
		}

		public void OnLocationChanged (Android.Locations.Location location)
		{
			Location = location.GetGeolocationFromAndroidLocation();
		}

		private void SetUpLocationRequest ()
		{
			if (_locationRequest == null) {
				_locationRequest = new LocationRequest ();
				_locationRequest.SetPriority (LocationRequest.PriorityHighAccuracy);
				_locationRequest.SetNumUpdates (NumberOfUpdates);
			}
		}

		private void SendMessage ()
		{
			if (MessengerClient != null) {
				DroidMessanging.SendMessage (ServiceConstants.ServicePushLocations, MessengerClient, null, Location.GetGeolocationFromAndroidLocation ());
			}
		}

		private void GetLocation ()
		{
			if (!GoogleApiClient.IsConnected) {
				GoogleApiClient.Connect ();
				return;
			}
			if (LocationServices.FusedLocationApi.GetLocationAvailability (GoogleApiClient).IsLocationAvailable) {
				Location = LocationServices.FusedLocationApi.GetLastLocation (GoogleApiClient).GetGeolocationFromAndroidLocation();
			}
			else {
				SetUpLocationRequest ();
				LocationServices.FusedLocationApi.RequestLocationUpdates (GoogleApiClient, _locationRequest, this);
			}
		}
	}	
}