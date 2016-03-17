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
	[Service(Enabled = true, IsolatedProcess = true, Exported = false)]
	[IntentFilter (new string [] { "com.tollminder.GeolocationService" })]
	public class GeolocationService : GoogleApiService<GeolocationServiceHandler>,
										Android.Gms.Location.ILocationListener									
										 
	{
		private LocationRequest _request;
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
			StopLocationUpdate ();
		}

		public virtual void StopLocationUpdate ()
		{
			if (GoogleApiClient != null && GoogleApiClient.IsConnected) {
				LocationServices.FusedLocationApi.RemoveLocationUpdates (GoogleApiClient, this);
			}
		}

		public virtual void StartLocationUpdate (bool fastUpdates = false)
		{
			Log.LogMessage ("START LOCATION UPDATES " + fastUpdates);				
			LocationServices.FusedLocationApi.RequestLocationUpdates (GoogleApiClient, LocationRequest , this);
		}

		public override void OnConnected (Bundle connectionHint)
		{
			base.OnConnected (connectionHint);
			StartLocationUpdate ();
			#if DEBUG
			Log.LogMessage("GoogleApiClient connected");
			#endif
		}

		public override void OnConnectionFailed (Android.Gms.Common.ConnectionResult result)
		{
			base.OnConnectionFailed (result);
		}

		public override void OnConnectionSuspended (int cause)
		{
			base.OnConnectionSuspended (cause);
		}

		public void OnLocationChanged (Location location)
		{
			Location = location.GetGeolocationFromAndroidLocation();
		}

		protected virtual LocationRequest LocationRequest
		{	
			get {
				if (_request != null) {
					return _request;
				}
				_request = new LocationRequest ();
				_request.SetPriority (LocationRequest.PriorityHighAccuracy);
				_request.SetInterval (1000);
				_request.SetFastestInterval (1000);
				return _request;
			}
		}

		private void SendMessage ()
		{
			if (MessengerClient != null) {
				DroidMessanging.SendMessage (ServiceConstants.ServicePushLocations, MessengerClient, null, Location.GetBundleFromLocation ());
			}
		}
	}	
}