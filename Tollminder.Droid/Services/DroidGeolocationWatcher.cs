﻿using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using Android.App;
using Android.Locations;
using Android.Content;
using Android.Widget;
using Android.OS;
using Cirrious.CrossCore;
using Android.Gms.Location;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Tollminder.Droid.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tollminder.Droid.Services
{
	[Service]
	public class DroidGeolocationWatcher : Service,
										Android.Gms.Common.Apis.GoogleApiClient.IConnectionCallbacks,
										Android.Gms.Common.Apis.GoogleApiClient.IOnConnectionFailedListener,
										Android.Gms.Location.ILocationListener,
										IResultCallback,
										IGeoLocationWatcher 
	{
		#region private fields

		private const int GeoFenceRadius = 100;

		public static readonly string GeoFenceRegionKey = "geoCurrentRegionPoint";

		const int NumberOfUpdates = 1;

		// Milliseconds per second
		private static readonly int MillescondPerSecond = 1000;
		// Update frequency in seconds
		private static readonly int UpdateIntervalInSeconds = 3;
		// Update frequency in milliseconds
		public static readonly long UpdateInterval = MillescondPerSecond * UpdateIntervalInSeconds;
		// The fastest update frequency, in seconds
		private static readonly int FastestIntervalInSeconds = 3;
		// A fast frequency ceiling in milliseconds
		public static readonly long FastestInterval = MillescondPerSecond * FastestIntervalInSeconds;

		private GoogleApiClient _locationClient;
		private LocationRequest _locationRequest;
		private IGeofence _geoFence;
		private GeofencingRequest _geoFenceRequest;
		private PendingIntent _geofencePendingIntent;

		// Flag that indicates if a request is underway.
		private bool _inProgress;

		#endregion

		public override void OnCreate ()
		{
			base.OnCreate ();
			SetupLocationService ();

		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			DestroyLocationService ();
		}

		#region implemented abstract members of Service


		public override IBinder OnBind (Intent intent)
		{
			return null;
		}


		#endregion


		public void OnResult (Java.Lang.Object result)
		{
			var asd = "somes ";
		}

		public void OnConnected (Bundle connectionHint)
		{
			if (!_locationClient.IsConnected) {
				_locationClient.Connect ();
				return;
			}
			_inProgress = true;
			if (LocationServices.FusedLocationApi.GetLocationAvailability (_locationClient).IsLocationAvailable) {
				Location = GetGeoLocationFromAndroidLocation (LocationServices.FusedLocationApi.GetLastLocation (_locationClient));				
				SetupGeofence ();
			} else {
				LocationServices.FusedLocationApi.RequestLocationUpdates (_locationClient, _locationRequest, this);
			}
		}

		public void OnConnectionSuspended (int cause)
		{			
		}

		public void OnConnectionFailed (Android.Gms.Common.ConnectionResult result)
		{
		}

		public void OnLocationChanged (Android.Locations.Location location)
		{
			Location = GetGeoLocationFromAndroidLocation (location);
			SetupGeofence ();
		}

		public void OnProviderDisabled (string provider)
		{			
		}

		public void OnProviderEnabled (string provider)
		{			
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{			
		}

		#region IGeoLocationWatcher implementation
		public event EventHandler<LocationUpdatedEventArgs> LocationUpdatedEvent;
		public void StartGeolocationWatcher ()
		{
			
		}
		public GeoLocation Location { get; set; }
		#endregion

		#region Helpers

		private void SetupGeofence ()
		{
			if (!Location.IsUnknownGeoLocation) {
				try {
					AddGeofencePoint (Location.Latitude, Location.Longitude);
					BuildGeofenceRequest ();
					LocationServices.GeofencingApi.AddGeofences (_locationClient, _geoFenceRequest, GetGeofencePendingIntent ()).SetResultCallback (this);					
				} catch (GeofenceException ex) {
					switch (ex.Status) {
					case GeofenceStatus.OnAddGeofencePoint:
						AddGeofencePoint (Location.Latitude, Location.Longitude);
						break;
					case GeofenceStatus.OnBuildGeoFenceRequest:
						BuildGeofenceRequest ();
						break;
					case GeofenceStatus.OnAddGeofence:
						LocationServices.GeofencingApi.AddGeofences (_locationClient, _geoFenceRequest, GetGeofencePendingIntent ()).SetResultCallback (this);					
						break;
					case GeofenceStatus.None:						
					default:
						break;
					}
				}
			} else {
				_locationClient.Connect ();
			}
		}

		private bool ServicesConnected()
		{
			// Check that Google Play services is available
			int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			// If Google Play services is available
			if (ConnectionResult.Success == resultCode) {
				return true;
			} else {
				return false;
			}
		}

		private GeoLocation GetGeoLocationFromAndroidLocation (Location loc)
		{
			var geoLocation = new GeoLocation (); 
			try {
				geoLocation.Accuracy = loc.Accuracy;
				geoLocation.Altitude = loc.Altitude;
				geoLocation.Longitude = loc.Longitude;
				geoLocation.Latitude = loc.Latitude;
				geoLocation.Speed = loc.Speed;
				#if DEBUG
				Toast.MakeText (this, geoLocation.ToString(), ToastLength.Short).Show();
				#endif								
			} catch (NullReferenceException ex) {
				_locationClient.Connect ();
			} catch (Exception exc) {
				
			}
			return geoLocation;
		}


		private void AddGeofencePoint (double lat, double lon)
		{
			try {
				_geoFence = new GeofenceBuilder ()
					.SetRequestId (GeoFenceRegionKey)
					.SetExpirationDuration (long.MaxValue)
					.SetCircularRegion (lat, lon, GeoFenceRadius)
					.SetTransitionTypes (Geofence.GeofenceTransitionExit | Geofence.GeofenceTransitionEnter)
					.Build ();				
			} catch (Exception ex) {
				#if DEBUG
				Mvx.Trace (ex.Message, string.Empty);
				#endif
				throw new GeofenceException (ex.Message, GeofenceStatus.OnAddGeofencePoint);
			} 							
		}

		private void SetupLocationService ()
		{
			if (_locationRequest == null) {
				_locationRequest = new LocationRequest ();
				_locationRequest.SetPriority (LocationRequest.PriorityHighAccuracy);
				_locationRequest.SetNumUpdates (NumberOfUpdates);				
			}
			if (_locationClient == null) {
				_locationClient = new GoogleApiClient.Builder (this).AddApi (LocationServices.API).AddConnectionCallbacks (this).AddOnConnectionFailedListener (this).Build ();
			}
			_locationClient.Connect ();				
		}

		private void DestroyLocationService ()
		{
			if (_locationClient != null) {				
				_locationClient.Disconnect ();
				_locationClient.UnregisterConnectionCallbacks (this);
				_locationClient.UnregisterConnectionFailedListener (this);
				_locationClient = null;
			}
		}

		private void RemoveGeofence ()
		{
			try {
				LocationServices.GeofencingApi.RemoveGeofences (_locationClient, GetGeofencePendingIntent ()).SetResultCallback(this);				
			} catch (Exception ex) {
				#if DEBUG
				Mvx.Trace (ex.Message, string.Empty);
				#endif
				throw new GeofenceException (ex.Message, GeofenceStatus.OnRemoveGeofence);
			}
		}

		private void BuildGeofenceRequest ()
		{
			try {
				_geoFenceRequest = new GeofencingRequest.Builder ().AddGeofence (_geoFence).SetInitialTrigger (GeofencingRequest.InitialTriggerExit).Build ();				
			} catch (Exception ex) {
				#if DEBUG
				Mvx.Trace (ex.Message, string.Empty);
				#endif
				throw new GeofenceException (ex.Message, GeofenceStatus.OnBuildGeoFenceRequest);
			}
		}

		private PendingIntent GetGeofencePendingIntent ()
		{
			if (_geofencePendingIntent != null) {
				return _geofencePendingIntent;
			}
			Intent intent = new Intent (this, typeof(GeofenceTransitionsIntentService));			 
			return PendingIntent.GetService (this, GeofenceTransitionsIntentService.GeofenceTransitionsIntentServiceCode, intent, PendingIntentFlags.UpdateCurrent);

		}
		#endregion		
	}
}