using System;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Cirrious.CrossCore;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;

namespace Tollminder.Droid.AndroidServices
{
	[Service]
	public class GeofenceService : GeolocationService
	{
		public static readonly string GeoFenceRegionKey = "geoCurrentRegionPoint";

		private const int GeoFenceRadius = 100;

		#region Private Fields
		private GeofencingRequest _geoFenceRequest;
		private IGeofence _geoFence;
		private PendingIntent _geofencePendingIntent;
		#endregion

		public override GeoLocation Location {
			get {
				return base.Location;
			}
			protected set {
				base.Location = value;
				SetUpGeofenicng (value);
			}
		}

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			var geofencingEvent = GeofencingEvent.FromIntent (intent);
			if (geofencingEvent.HasError) {
				return StartCommandResult.Sticky;
			}
			int geofenceTransition = geofencingEvent.GeofenceTransition;

			if (geofenceTransition == Geofence.GeofenceTransitionExit) {
				Location = geofencingEvent.TriggeringLocation.GetGeolocationFromAndroidLocation ();
			}
			return StartCommandResult.Sticky;
		}

		#region Helpers
		public async void SetUpGeofenicng (GeoLocation location)
		{
			if (!Location.IsUnknownGeoLocation) {
				try {
					if (_geoFence != null) {
						RemoveGeofence();
					}
					AddGeofencePoint (location.Latitude, location.Longitude);
					BuildGeofenceRequest ();				
					var status = await LocationServices.GeofencingApi.AddGeofencesAsync (GoogleApiClient, _geoFenceRequest, GetGeofencePendingIntent ());
					#if DEBUG
					Log.LogMessage(string.Format ("Added to location Services --- {0}", status.Status.IsSuccess));
					#endif
				} catch (GeofenceException ex) {
					switch (ex.Status) {
					case GeofenceStatus.OnAddGeofencePoint:
						AddGeofencePoint (location.Latitude, location.Longitude);
						break;
					case GeofenceStatus.OnBuildGeoFenceRequest:
						BuildGeofenceRequest ();
						break;
					case GeofenceStatus.OnAddGeofence:
						await LocationServices.GeofencingApi.AddGeofencesAsync (GoogleApiClient, _geoFenceRequest, GetGeofencePendingIntent ());
						break;
					case GeofenceStatus.None:						
					default:
						break;
					}
				} catch (Exception ex) {
					#if DEBUG
					Log.LogMessage(ex.Message);
					#endif
				}
			} else {
				GoogleApiClient.Connect ();
			}
		}

		private void AddGeofencePoint (double lat, double lon)
		{
			try {
				_geoFence = new GeofenceBuilder ()
					.SetRequestId (GeoFenceRegionKey)
					.SetExpirationDuration (Geofence.NeverExpire)
					.SetCircularRegion (lat, lon, GeoFenceRadius)
					.SetTransitionTypes (Geofence.GeofenceTransitionExit | Geofence.GeofenceTransitionEnter)
					.Build ();
				#if DEBUG
				Log.LogMessage("Geolocation added");
				#endif
			} catch (Exception ex) {
				#if DEBUG
				Log.LogMessage (ex.Message);
				#endif
				throw new GeofenceException (ex.Message, GeofenceStatus.OnAddGeofencePoint);
			} 							
		}

		public async void RemoveGeofence ()
		{
			try {
				var result = await LocationServices.GeofencingApi.RemoveGeofencesAsync (GoogleApiClient, GetGeofencePendingIntent ());
				#if DEBUG
				Log.LogMessage(string.Format ("Geofence removed --- {0}", result.Status.IsSuccess));
				#endif
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
				#if DEBUG
				Log.LogMessage("New request was builded");
				#endif
			} catch (Exception ex) {
				#if DEBUG
				Log.LogMessage(ex.Message);
				#endif
				throw new GeofenceException (ex.Message, GeofenceStatus.OnBuildGeoFenceRequest);
			}
		}

		private PendingIntent GetGeofencePendingIntent ()
		{
			if (_geofencePendingIntent != null) {
				return _geofencePendingIntent;
			}
			Intent intent = new Intent (this, typeof(GeolocationService));
			_geofencePendingIntent = PendingIntent.GetService (this, 0, intent, PendingIntentFlags.UpdateCurrent);
			return _geofencePendingIntent;
		}
		#endregion
	}
}