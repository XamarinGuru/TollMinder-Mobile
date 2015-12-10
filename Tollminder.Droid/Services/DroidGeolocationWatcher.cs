using System;
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
using Android.Support.V4.App;
using Android.Graphics;
using Tollminder.Droid.Views;
using MessengerHub;

namespace Tollminder.Droid.Services
{
	[Service]
	public class DroidGeolocationWatcher : Service,
										Android.Gms.Common.Apis.GoogleApiClient.IConnectionCallbacks,
										Android.Gms.Common.Apis.GoogleApiClient.IOnConnectionFailedListener,
										Android.Gms.Location.ILocationListener,										
										IGeoLocationWatcher 
	{
		IMessengerHub _messengerHub;

		public DroidGeolocationWatcher ()
		{
			this._messengerHub = Mvx.Resolve<IMessengerHub> ();
			
		}
		#region private fields

		private const int GeoFenceRadius = 1000;

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

		#endregion

		#region implemented abstract members of IntentService

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			var geofencingEvent = GeofencingEvent.FromIntent (intent);
			if (geofencingEvent.HasError) {
				return StartCommandResult.ContinuationMask;
			}
			int geofenceTransition = geofencingEvent.GeofenceTransition;

			if (geofenceTransition == Geofence.GeofenceTransitionExit) {

				string geofenceTransitionDetails = GetGeofenceTransitionDetails (this, geofenceTransition, geofencingEvent.TriggeringGeofences);

				SendNotification (geofenceTransitionDetails);

				Location = geofencingEvent.TriggeringLocation.GetGeolocationFromAndroidLocation ();
			}
			return base.OnStartCommand (intent, flags, startId);
		}

		string GetGeofenceTransitionDetails (Context context, int geofenceTransition, IList<IGeofence> triggeringGeofences)
		{
			string geofenceTransitionString = GetTransitionString (geofenceTransition);

			var triggeringGeofencesIdsList = new List<string> ();
			foreach (IGeofence geofence in triggeringGeofences) {
				triggeringGeofencesIdsList.Add (geofence.RequestId);
			}
			var triggeringGeofencesIdsString = string.Join (", ", triggeringGeofencesIdsList);

			return string.Format ("{0}: {1}",geofenceTransitionString, triggeringGeofencesIdsString);
		}

		void SendNotification (string notificationDetails)
		{
			Mvx.Resolve<INotificationSender> ().SendLocalNotification ("EXIT", notificationDetails);
		}

		string GetTransitionString (int transitionType)
		{
			switch (transitionType) {
			case Geofence.GeofenceTransitionEnter:
				return "ENTER";
			case Geofence.GeofenceTransitionExit:
				return "EXIT";
			default:
				return "What do you mean ?";
			}
		}

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
			RemoveGeofence ();
		}

		#region implemented abstract members of Service

		public override IBinder OnBind (Intent intent)
		{
			return null;
		}

		#endregion

		public void OnConnected (Bundle connectionHint)
		{
			GetLocation ();
		}

		public void OnConnectionSuspended (int cause)
		{			
			Log.LogMessage (cause.ToString());
		}

		public void OnConnectionFailed (Android.Gms.Common.ConnectionResult result)
		{
			Log.LogMessage (result.ErrorMessage);
		}

		public void OnLocationChanged (Android.Locations.Location location)
		{
			Location = GetGeoLocationFromAndroidLocation (location);
		}

		public void OnProviderDisabled (string provider)
		{			
			Log.LogMessage (provider);
		}

		public void OnProviderEnabled (string provider)
		{			
			Log.LogMessage (provider);
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{		
			Log.LogMessage (provider);	
		}

		public event EventHandler<LocationUpdatedEventArgs> LocationUpdatedEvent;
		public void StartGeolocationWatcher ()
		{
			
		}
		private GeoLocation _geoLocation;
		public GeoLocation Location {
			get { return _geoLocation; } 
			set { 
				_geoLocation = value;
				_messengerHub.Publish (new LocationUpdatedMessage (this, _geoLocation));
				SetUpGeofenicng (Location);
			}
		}

		#region Helpers

		private void GetLocation ()
		{
			if (!_locationClient.IsConnected) {
				_locationClient.Connect ();
				return;
			}
			if (LocationServices.FusedLocationApi.GetLocationAvailability (_locationClient).IsLocationAvailable) {
				Location = GetGeoLocationFromAndroidLocation (LocationServices.FusedLocationApi.GetLastLocation (_locationClient));
			}
			else {
				LocationServices.FusedLocationApi.RequestLocationUpdates (_locationClient, _locationRequest, this);
			}
		}

		public async void SetUpGeofenicng (GeoLocation location)
		{
			if (!Location.IsUnknownGeoLocation) {
				try {
					if (_geoFence != null) {
						RemoveGeofence();
					}
					AddGeofencePoint (location.Latitude, location.Longitude);
					BuildGeofenceRequest ();				
					await LocationServices.GeofencingApi.AddGeofences (_locationClient, _geoFenceRequest, GetGeofencePendingIntent ());
				} catch (GeofenceException ex) {
					switch (ex.Status) {
					case GeofenceStatus.OnAddGeofencePoint:
						AddGeofencePoint (location.Latitude, location.Longitude);
						break;
					case GeofenceStatus.OnBuildGeoFenceRequest:
						BuildGeofenceRequest ();
						break;
					case GeofenceStatus.OnAddGeofence:
						await LocationServices.GeofencingApi.AddGeofencesAsync (_locationClient, _geoFenceRequest, GetGeofencePendingIntent ());
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
				geoLocation = loc.GetGeolocationFromAndroidLocation();
				#if DEBUG
				Log.LogMessage(geoLocation.ToString());
				#endif							
			} catch (NullReferenceException) {
				_locationClient.Connect ();
			} catch (Exception ex) {
				#if DEBUG
				Log.LogMessage(ex.Message);
				#endif
			}
			return geoLocation;
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
			} catch (Exception ex) {
				#if DEBUG
				Log.LogMessage (ex.Message);
				#endif
				throw new GeofenceException (ex.Message, GeofenceStatus.OnAddGeofencePoint);
			} 							
		}

		private void SetUpLocationRequest ()
		{
			if (_locationRequest == null) {
				_locationRequest = new LocationRequest ();
				_locationRequest.SetPriority (LocationRequest.PriorityHighAccuracy);
				_locationRequest.SetNumUpdates (NumberOfUpdates);
			}
		}

		private void SetUpLocationClient ()
		{
			if (_locationClient == null) {
				_locationClient = new GoogleApiClient.Builder (this).AddApi (LocationServices.API).AddConnectionCallbacks (this).AddOnConnectionFailedListener (this).Build ();
			}
		}

		public void SetupLocationService ()
		{
			SetUpLocationRequest ();
			SetUpLocationClient ();
			_locationClient.Connect ();				
		}

		public void DestroyLocationService ()
		{
			if (_locationClient != null) {				
				_locationClient.Disconnect ();
				_locationClient.UnregisterConnectionCallbacks (this);
				_locationClient.UnregisterConnectionFailedListener (this);
				_locationClient = null;
			}
		}

		public async void RemoveGeofence ()
		{
			try {
				await LocationServices.GeofencingApi.RemoveGeofences (_locationClient, GetGeofencePendingIntent ());
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
			Intent intent = new Intent (this, typeof(DroidGeolocationWatcher));
			return PendingIntent.GetService (this, 0, intent, PendingIntentFlags.UpdateCurrent);
		}
		#endregion
	}
}