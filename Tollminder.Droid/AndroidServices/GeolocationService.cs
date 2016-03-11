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
	public class GeolocationService : GoogleApiService<GeolocationServiceHandler>,
										Android.Gms.Location.ILocationListener									
										 
	{
		private LocationRequest _fastRequest;
		private LocationRequest _slowRequest;
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
			LocationServices.FusedLocationApi.RequestLocationUpdates (GoogleApiClient, FastLocationUpdate , this);
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

		protected virtual LocationRequest FastLocationUpdate
		{	
			get {
				if (_fastRequest != null) {
					return _fastRequest;
				}
				Log.LogMessage ("FAST REQUEST WAS BUILD");
				_fastRequest = new LocationRequest ();
				_fastRequest.SetPriority (LocationRequest.PriorityHighAccuracy);
				_fastRequest.SetInterval (1000);
				_fastRequest.SetFastestInterval (1000);
				return _fastRequest;
			}
		}

		protected virtual LocationRequest SlowLocationRequest 
		{			
			get {
				if (_slowRequest != null) {
					return _slowRequest;
				}
				Log.LogMessage ("SLOW REQUEST WAS BUILD");
				_slowRequest = new LocationRequest ();
				_slowRequest.SetPriority (LocationRequest.PriorityHighAccuracy);
				_slowRequest.SetInterval (60000);
				_slowRequest.SetFastestInterval (60000);
				return _slowRequest;
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