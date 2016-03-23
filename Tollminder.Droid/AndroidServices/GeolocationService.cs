using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;

namespace Tollminder.Droid.AndroidServices
{
	[Service(Enabled = true, Exported = false)]
	public class GeolocationService : GoogleApiService
	{
		private const string IntervalString = "interval";
		private LocationRequest _request;
		private PendingIntent _geolocationPendingIntent;
		public virtual int Interval { get; set; }

		public override void OnCreate ()
		{
			base.OnCreate ();
			CreateGoogleApiClient (LocationServices.API);
			Connect ();
		}

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			Interval = intent.GetIntExtra (IntervalString, 20000);
			return StartCommandResult.RedeliverIntent;
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			StopLocationUpdate ();
			GeolocationPendingIntent.Cancel ();
		}

		public virtual void StopLocationUpdate ()
		{
			if (GoogleApiClient != null && GoogleApiClient.IsConnected) {
				LocationServices.FusedLocationApi.RemoveLocationUpdates (GoogleApiClient, GeolocationPendingIntent);
			}
		}

		public virtual void StartLocationUpdate ()
		{
			Log.LogMessage ("START LOCATION UPDATES ");				
			LocationServices.FusedLocationApi.RequestLocationUpdates (GoogleApiClient, LocationRequest , GeolocationPendingIntent);
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

		protected PendingIntent GeolocationPendingIntent {
			get {
				if (_geolocationPendingIntent != null) {
					return _geolocationPendingIntent;
				}
				Intent intent = new Intent ("com.tollminder.GeolocationReciever");
				intent.SetPackage ("com.tollminder");
				_geolocationPendingIntent = PendingIntent.GetBroadcast (this, 0, intent, PendingIntentFlags.UpdateCurrent);
				return _geolocationPendingIntent;
			}
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
				_request.se
				return _request;
			}
		}

		public override IBinder OnBind (Intent intent)
		{
			return null;
		}
	}	
}