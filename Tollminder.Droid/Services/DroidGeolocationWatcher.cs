using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Handlers;
using Tollminder.Droid.Helpers;
using Tollminder.Droid.ServicesConnections;
using Android.Gms.Common;
using System;

namespace Tollminder.Droid.Services
{
	public class DroidGeolocationWatcher :  AndroidServiceWithServiceConnection<GeofenceService,GeolocationClientHandler, GeolocationServiceConnection>, IGeoLocationWatcher
	{	
		#region IGeoLocationWatcher implementation

		public bool IsBound { get; private set; } = false;
		bool _geofenceEnabled = true;
		/// <summary>
		/// Gets or sets a value indicating whether geofence enabled.
		/// Default true.
		/// </summary>
		/// <value><c>true</c> if geofence enabled; otherwise, <c>false</c>.</value>
		public virtual bool GeofenceEnabled {
			get { return _geofenceEnabled;	}
			set {
				_geofenceEnabled = value;
				EnabledGeofenceService (value);
			}
		} 

		bool IsGooglePlayServicesInstalled
		{
			get {
				int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable (ApplicationContext);
				if (queryResult == ConnectionResult.Success) {					
					return true;
				}

				if (GoogleApiAvailability.Instance.IsUserResolvableError (queryResult)) {
					string errorString = GoogleApiAvailability.Instance.GetErrorString (queryResult);
					#if DEBUG
					Log.LogMessage(string.Format("There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString));
					#endif
				}
				return false;
			}
		}

		GeoLocation _location;
		public virtual GeoLocation Location {
			get {
				return _location;
			}
			set {
				_location = value;
				Mvx.Resolve<IMvxMessenger> ().Publish (new LocationMessage (this, Location));
				#if DEBUG
				Log.LogMessage (value.ToString ());
				#endif
			}
		}

		public virtual void StartGeolocationWatcher ()
		{	
			if (!IsBound & IsGooglePlayServicesInstalled) {
				Start ();
				IsBound = true;				
			}
		}

		public virtual void StopGeolocationWatcher ()
		{
			if (IsBound & MessengerService != null) {				
				Stop ();
				IsBound = false;
			}
		}

		public virtual void StartUpdatingHighAccuracyLocation()
		{			
			DroidMessanging.SendMessage (ServiceConstants.StartLocation, MessengerService, null);
		}

//		public virtual void StopUpdatingHighAccuracyLocation()
//		{			
//			DroidMessanging.SendMessage (ServiceConstants.StopLocation, MessengerService, null);
//		}

		public virtual void EnabledGeofenceService(bool isEnabled)
		{			
			Log.LogMessage (string.Format (" - --- - - -  {0}    --- - - - - - -", isEnabled));
			DroidMessanging.SendMessage (ServiceConstants.GeoFenceEnabled, MessengerService, null , isEnabled.GetBundle());
			DroidMessanging.SendMessage (ServiceConstants.StartLocation, MessengerService, null, (!isEnabled).GetBundle());
		}

		public void StopUpdatingHighAccuracyLocation ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}