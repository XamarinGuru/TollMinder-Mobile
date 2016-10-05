using System;
using System.Timers;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Core.Services.Implementation;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Services
{
	public class DroidGeolocationWatcher : DroidServiceStarter, IGeoLocationWatcher
	{
		readonly IStoredSettingsService _storedSettingsService;
		readonly IMvxMessenger _messenger;

		#region IGeoLocationWatcher implementation
		public DroidGeolocationWatcher (IStoredSettingsService storedSettingsService, IMvxMessenger messenger)
		{
			_storedSettingsService = storedSettingsService;
			_messenger = messenger;
			ServiceIntent = new Intent (ApplicationContext, typeof (GeolocationService));
		}

		public bool IsBound 
		{ 
			get
			{
				return _storedSettingsService.GeoWatcherIsRunning;
			}
			set
			{
				_storedSettingsService.GeoWatcherIsRunning = value;
				_messenger.Publish(new GeoWatcherStatusMessage(this, value));
			}
		}

		public virtual GeoLocation Location {
			get {
				return _storedSettingsService.Location;
			}
			set {
				Log.LogMessage($"Recieved new location in geolocation watcher {value}");
                if (IsBound && (!_storedSettingsService.Location?.Equals(value) ?? true))
				{
					_storedSettingsService.Location = value;

					Mvx.Resolve<IMvxMessenger>().Publish(new LocationMessage(this, value));
					Log.LogMessage($"New location {value}");
					_storedSettingsService.Location = value;
				}
			}
		}

		public virtual void StartGeolocationWatcher ()
		{
			if (!IsBound && ApplicationContext.IsGooglePlayServicesInstalled ()) {
				Log.LogMessage("StartGeolocationWatcher");
				Start ();
				IsBound = true;
			}
		}

		public virtual void StopGeolocationWatcher ()
		{
			if (IsBound) {
				Stop ();
				Log.LogMessage("StopGeolocationWatcher");
				IsBound = false;
			}
		}

		public virtual void StartUpdatingHighAccuracyLocation ()
		{
            UpdateAccuracyLocation(SettingsService.DistanceIntervalHighDefault);
		}

		public virtual void StopUpdatingHighAccuracyLocation ()
		{
			UpdateAccuracyLocation(SettingsService.DistanceIntervalDefault);
		}

		void UpdateAccuracyLocation(int distanceInterval)
		{
			if (IsBound)
			{
				Stop();
				ServiceIntent.PutExtra(GeolocationService._distanceIntervalString, distanceInterval);
				Start();
			}
		}

		#endregion


	}
}