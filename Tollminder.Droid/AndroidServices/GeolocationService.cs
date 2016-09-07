﻿using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.OS;
using Tollminder.Core.Helpers;
using Tollminder.Droid.BroadcastReceivers;
using Tollminder.Droid.Services;

namespace Tollminder.Droid.AndroidServices
{
	[Service (Enabled = true, Exported = false)]
	public class GeolocationService : GoogleApiService
	{
		private const string IntervalString = "interval";
		private LocationRequest _request;
		private PendingIntent _geolocationPendingIntent;
		private GeolocationReceiver _reciever;

		public virtual int Interval { get; set; }
		public GeolocationReceiver Reciever {
			get {
				if (_reciever == null) {
					_reciever = new GeolocationReceiver ();
				}
				return _reciever;
			}
		}

		public override void OnCreate ()
		{
			base.OnCreate ();
			var intentFilter = new IntentFilter ();
			intentFilter.AddAction ("com.tollminder.GeolocationReciever");
			RegisterReceiver (Reciever, intentFilter);
			CreateGoogleApiClient (LocationServices.API);
			Connect ();
		}

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			Interval = intent.GetIntExtra (IntervalString, 400);
			return StartCommandResult.RedeliverIntent;
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			UnregisterReceiver (Reciever);
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
			LocationServices.FusedLocationApi.RequestLocationUpdates (GoogleApiClient, LocationRequest, GeolocationPendingIntent);
		}

		public override void OnConnected (Bundle connectionHint)
		{
			base.OnConnected (connectionHint);
			StartLocationUpdate ();
#if DEBUG
			Log.LogMessage ("GoogleApiClient connected");
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

		protected virtual LocationRequest LocationRequest {
			get {
				if (_request != null) {
					return _request;
				}
				_request = new LocationRequest ();
				_request.SetPriority (LocationRequest.PriorityHighAccuracy);
				//TODO: Remove interval and need to be tested.
				_request.SetInterval (2000);
				_request.SetSmallestDisplacement (Interval);
				return _request;
			}
		}

		public override IBinder OnBind (Intent intent)
		{
			return null;
		}
	}
}