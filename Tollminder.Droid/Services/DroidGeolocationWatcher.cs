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

		private GeoLocation _location;
		public virtual GeoLocation Location {
			get {
				return _location;
			}
			set {
				if (IsBound && (!_location?.Equals(value) ?? true))
				{
					_location = value;

					Mvx.Resolve<IMvxMessenger>().Publish(new LocationMessage(this, value));
					Log.LogMessage(value.ToString());
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