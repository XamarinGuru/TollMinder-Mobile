using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using CoreLocation;
using UIKit;
using Tollminder.Core.Helpers;
using System.Linq;
using Cirrious.CrossCore;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
using MessengerHub;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : IGeoLocationWatcher
	{
		private readonly TouchGeoFence _geofence;
		public bool IsBound { get; private set; } = false;

		GeoLocation _location;
		public GeoLocation Location {
			get { return _location;	}
			set {
				_location = value;
				SpeakTextIfNeeded ();
				LocationUpdatedMessage ();
			}
		}

		public TouchGeolocationWatcher ()
		{
			_geofence = new TouchGeoFence ();
		}

		#region IGeoLocationWatcher implementation

		public void StartGeolocationWatcher()
		{
			if (!IsBound) {
				_geofence.StartGeofenceService ();
				IsBound = true;				
			}	
		}

		public void StopGeolocationWatcher ()
		{
			if (IsBound) {
				_geofence.StopGeofenceService ();
				IsBound = false;				
			}
		}

		#endregion

		public void SpeakTextIfNeeded ()
		{
			Mvx.Resolve<ITextToSpeechService> ().Speak (string.Format ("Location Update at {0:t}", DateTime.Now));
		}

		private void LocationUpdatedMessage ()
		{
			Mvx.Resolve<IMessengerHub> ().Publish (new LocationUpdatedMessage (this, _location));
			if (!Mvx.Resolve<IPlatform> ().IsAppInForeground) {
				Mvx.Resolve<INotificationSender> ().SendLocalNotification ("LOCATION UPDATED", _location.ToString ());
			}
		}	
	}
}