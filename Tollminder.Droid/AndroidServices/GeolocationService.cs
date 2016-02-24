using Android.App;
using Android.Gms.Location;
using Android.OS;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Droid.Helpers;
using Tollminder.Droid.Handlers;
using Android.Locations;

namespace Tollminder.Droid.AndroidServices
{	
	
	public class GeolocationService : GoogleApiService<GeolocationServiceHandler>,
										Android.Gms.Location.ILocationListener									
										 
	{
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

		public override void OnDestroy ()
		{
			base.OnDestroy ();
		}

		public virtual void StopLocationUpdate ()
		{
			if (GoogleApiClient != null && GoogleApiClient.IsConnected) {
				LocationServices.FusedLocationApi.RemoveLocationUpdates (GoogleApiClient, this);
			}
		}

		public virtual void StartLocationUpdate ()
		{
			Connect ();
//			StopLocationUpdate ();
			SetUpLocationRequest ();
			LocationServices.FusedLocationApi.RequestLocationUpdates (GoogleApiClient, _locationRequest, this);
		}

		public override void OnConnected (Bundle connectionHint)
		{
			base.OnConnected (connectionHint);
			StartLocationUpdate ();
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
				_locationRequest.SetInterval(0);
				_locationRequest.SetFastestInterval(0);
			}
		}

		private void SendMessage ()
		{
			if (MessengerClient != null) {
				DroidMessanging.SendMessage (ServiceConstants.ServicePushLocations, MessengerClient, null, Location.GetGeolocationFromAndroidLocation ());
			}
		}
	}	
}