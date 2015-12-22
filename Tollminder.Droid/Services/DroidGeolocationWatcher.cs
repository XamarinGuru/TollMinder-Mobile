using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Android.Content;
using Tollminder.Droid.Helpers;
using Android.OS;
using Tollminder.Core.Helpers;
using MessengerHub;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Handlers;
using Tollminder.Droid.ServicesConnections;

namespace Tollminder.Droid.Services
{
	public class DroidGeolocationWatcher :  AndroidServiceWithServiceConnection<GeofenceService,GeolocationClientHandler,BaseServiceConnection>, IGeoLocationWatcher
	{	
		public bool IsBound { get; private set; } = false;

		#region IGeoLocationWatcher implementation
		GeoLocation _location;
		public GeoLocation Location {
			get {
				return _location;
			}
			set {
				_location = value;
				#if DEBUG
				Log.LogMessage (value.ToString ());
				#endif
				SpeakTextIfNeeded ();
				LocationUpdatedMessage ();
			}
		}

		public void StartGeolocationWatcher ()
		{	
			if (!IsBound) {
				Start ();
				IsBound = true;				
			}
		}

		public void StopGeolocationWatcher ()
		{
			if (IsBound & MessengerService != null) {
				Stop ();
				IsBound = false;
			}
		}
		#endregion

		void LocationUpdatedMessage ()
		{
			Mvx.Resolve<IMessengerHub> ().Publish (new LocationUpdatedMessage (this, _location));
			if (!Mvx.Resolve<IPlatform> ().IsAppInForeground) {
				Mvx.Resolve<INotificationSender> ().SendLocalNotification ("LOCATION UPDATED", _location.ToString ());
			}
		}

		public void SpeakTextIfNeeded ()
		{
			Mvx.Resolve<ITextToSpeechService> ().Speak (string.Format ("Location Update at {0:t}", DateTime.Now));
		}
	}
}