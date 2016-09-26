using System;
using System.Timers;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Services
{
	public class DroidGeolocationWatcher : DroidServiceStarter, IGeoLocationWatcher
	{
		readonly IStoredSettingsService _storedSettingsService;

		#region IGeoLocationWatcher implementation
		public DroidGeolocationWatcher (IStoredSettingsService storedSettingsService)
		{
			_storedSettingsService = storedSettingsService;

			ServiceIntent = new Intent (ApplicationContext, typeof (GeolocationService));
			IsBound = Mvx.Resolve<IStoredSettingsService>().GeoWatcherIsRunning;
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
			}
		}

		public virtual GeoLocation Location {
			get {
				return _storedSettingsService.Location;
			}
			set {
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
				Start ();
				IsBound = true;
			}
		}

		public virtual void StopGeolocationWatcher ()
		{
			if (IsBound) {
				Stop ();
				IsBound = false;
			}
		}

		public virtual void StartUpdatingHighAccuracyLocation ()
		{
			UpdateAccuracyLocation(20);
		}

		public virtual void StopUpdatingHighAccuracyLocation ()
		{
			UpdateAccuracyLocation(GeolocationService._distanceIntervalDefault);
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