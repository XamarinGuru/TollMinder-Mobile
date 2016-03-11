using System;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using MvvmCross.Platform;
using System.Threading.Tasks;
using Android.Gms.Common;
using System.Linq;

namespace Tollminder.Droid.AndroidServices
{
	[Service(Enabled = true, IsolatedProcess = true, Exported = false)]
	public class GeofenceService : GeolocationService
	{
		public static readonly string GeoFenceRegionKey = "geoCurrentRegionPoint";


		#region Private Fields
		private const int GeoFenceRadius = 500;
		private PendingIntent _geofencePendingIntent;
		bool _geofenceEnabled = true;
		#endregion


		public bool GeofenceEnabled {
			get { return _geofenceEnabled; }
			set {
				_geofenceEnabled = value;
				if (!_geofenceEnabled) 
					RemoveGeofence ();
			}
		} 

		public override GeoLocation Location {
			get { return base.Location;	}
			protected set {
				base.Location = value;
				if (GeofenceEnabled) {					
					SetUpGeofenicng ();
				}
			}
		}

		public override void OnDestroy ()
		{
			RemoveGeofence ();
			base.OnDestroy ();
		}

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			Log.LogMessage ("GEOFENCE TRIGGERED GEOFENCE TRIGGERED GEOFENCE TRIGGERED GEOFENCE TRIGGERED GEOFENCE TRIGGERED");
			var geofencingEvent = GeofencingEvent.FromIntent (intent);
			if (geofencingEvent.TriggeringGeofences.All (x => x.RequestId != GeoFenceRegionKey)) {
				Log.LogMessage ("THERE IS NO OURS TRIGGERING REGIONS");
				return StartCommandResult.Sticky;
			}
			if (geofencingEvent.HasError) {
				Log.LogMessage ("GEOFENCE HAS A ERROR");
				return StartCommandResult.Sticky;
			}
			int geofenceTransition = geofencingEvent.GeofenceTransition;

			if (geofenceTransition == Geofence.GeofenceTransitionExit) {	
				Log.LogMessage ("TRANSITION EXIT");
				Location = geofencingEvent.TriggeringLocation.GetGeolocationFromAndroidLocation ();
			}
			return StartCommandResult.Sticky;
		}

		#region Helpers
		protected virtual void SetUpGeofenicng ()
		{								
			LocationServices.GeofencingApi.AddGeofencesAsync (GoogleApiClient, BuildGeofenceRequest, GetGeofencePendingIntent);	
		}

		public void RemoveGeofence ()
		{
			try {
				LocationServices.GeofencingApi.RemoveGeofences (GoogleApiClient, GetGeofencePendingIntent);
			} catch (Exception ex) {
				#if DEBUG
				Mvx.Trace (ex.Message, string.Empty);
				#endif
			}
		}

		protected GeofencingRequest BuildGeofenceRequest
		{
			get{
				#if DEBUG
				Log.LogMessage ("NEW REQUEST WAS BUILDED");
				#endif
				return new GeofencingRequest.Builder ().AddGeofence (GeofencePoint).SetInitialTrigger (GeofencingRequest.InitialTriggerExit).Build ();
			}
		}

		protected PendingIntent GetGeofencePendingIntent 
		{
			get {
				if (_geofencePendingIntent != null) {
					return _geofencePendingIntent;
				}
				Intent intent = new Intent (this, typeof(GeofenceService));
				_geofencePendingIntent = PendingIntent.GetService (this, 101, intent, PendingIntentFlags.UpdateCurrent);
				return _geofencePendingIntent;
			}
		}

		protected IGeofence GeofencePoint 
		{
			get {
				#if DEBUG
				Log.LogMessage ("CREATE NEW GEOFENCE POINT");
				#endif				
				return new GeofenceBuilder ()
					.SetRequestId (GeoFenceRegionKey)
					.SetExpirationDuration (Geofence.NeverExpire)
					.SetNotificationResponsiveness (3000)
					.SetCircularRegion (Location.Latitude, Location.Longitude, GeoFenceRadius)
					.SetTransitionTypes (Geofence.GeofenceTransitionExit)
					.Build ();
			}
		}
		#endregion
	}
}