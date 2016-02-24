﻿using System;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using MvvmCross.Platform;
using System.Threading.Tasks;

namespace Tollminder.Droid.AndroidServices
{
	[Service]
	public class GeofenceService : GeolocationService
	{
		public static readonly string GeoFenceRegionKey = "geoCurrentRegionPoint";

		private const int GeoFenceRadius = 200;

		#region Private Fields
		private GeofencingRequest _geoFenceRequest;
		private IGeofence _geoFence;
		private PendingIntent _geofencePendingIntent;
		#endregion


		bool _geofenceEnabled = true;
		public bool GeofenceEnabled {
			get {
				return _geofenceEnabled;
			}
			set {
				_geofenceEnabled = value;
				if (!_geofenceEnabled) 
					RemoveGeofence ();
			}
		} 

		public override GeoLocation Location {
			get {
				return base.Location;
			}
			protected set {
				base.Location = value;
				if (GeofenceEnabled) {
					StopLocationUpdate ();
					SetUpGeofenicng (value);
				}
			}
		}

		public override async void OnDestroy ()
		{
			await RemoveGeofence ().ConfigureAwait (false);
			base.OnDestroy ();
		}

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			var geofencingEvent = GeofencingEvent.FromIntent (intent);
			if (geofencingEvent.HasError) {
				return StartCommandResult.Sticky;
			}
			int geofenceTransition = geofencingEvent.GeofenceTransition;

			if (geofenceTransition == Geofence.GeofenceTransitionExit) {				
//				Location = geofencingEvent.TriggeringLocation.GetGeolocationFromAndroidLocation ();
				StartLocationUpdate ();
			}
			return StartCommandResult.Sticky;
		}

		#region Helpers
		protected virtual async void SetUpGeofenicng (GeoLocation location)
		{
			if (!Location.IsUnknownGeoLocation) {
				try {
					if (_geoFence != null) {						
						await RemoveGeofence().ConfigureAwait(false);
					}
					AddGeofencePoint (location.Latitude, location.Longitude);
					BuildGeofenceRequest ();				
					var status = await LocationServices.GeofencingApi.AddGeofencesAsync (GoogleApiClient, _geoFenceRequest, GetGeofencePendingIntent ());
					#if DEBUG
					Log.LogMessage(string.Format ("ADDED TO GEOFENCE NEW LOCATION --- {0}", status.Status.IsSuccess));
					#endif				
				} catch (Exception ex) {
					#if DEBUG
					Log.LogMessage(ex.Message);
					#endif
				}
			} else {
				StartLocationUpdate ();
			}
		}

		private void AddGeofencePoint (double lat, double lon)
		{
			try {
				_geoFence = new GeofenceBuilder ()
					.SetRequestId (GeoFenceRegionKey)
					.SetExpirationDuration (Geofence.NeverExpire)
					.SetNotificationResponsiveness(1000)
					.SetCircularRegion (lat, lon, GeoFenceRadius)
					.SetTransitionTypes (Geofence.GeofenceTransitionExit)
					.Build ();
				#if DEBUG
				Log.LogMessage("CREATE NEW GEOFENCE POINT");
				#endif
			} catch (Exception ex) {
				#if DEBUG
				Log.LogMessage (ex.Message);
				#endif
			} 							
		}

		public async Task RemoveGeofence ()
		{
			try {
				var result = await LocationServices.GeofencingApi.RemoveGeofencesAsync (GoogleApiClient, GetGeofencePendingIntent ());
				#if DEBUG
				Log.LogMessage(string.Format ("GEOFENCE REMOVED --- {0}", result.Status.IsSuccess));
				#endif
			} catch (Exception ex) {
				#if DEBUG
				Mvx.Trace (ex.Message, string.Empty);
				#endif
			}
		}

		private void BuildGeofenceRequest ()
		{
			try {
				_geoFenceRequest = new GeofencingRequest.Builder ().AddGeofence (_geoFence).SetInitialTrigger (GeofencingRequest.InitialTriggerExit|GeofencingRequest.InitialTriggerEnter).Build ();
				#if DEBUG
				Log.LogMessage("NEW REQUEST WAS BUILDED");
				#endif
			} catch (Exception ex) {
				#if DEBUG
				Log.LogMessage(ex.Message);
				#endif
			}
		}

		private PendingIntent GetGeofencePendingIntent ()
		{
			if (_geofencePendingIntent != null) {
				return _geofencePendingIntent;
			}
			Intent intent = new Intent (this, typeof(GeofenceService));
			_geofencePendingIntent = PendingIntent.GetService (this, 0, intent, PendingIntentFlags.UpdateCurrent);
			return _geofencePendingIntent;
		}
		#endregion
	}
}