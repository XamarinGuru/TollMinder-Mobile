using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;
using System;
using System.Timers;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : TouchLocation, IGeoLocationWatcher
	{	
		readonly IStoredSettingsService _storedSettingsService;
		readonly IMvxMessenger _messenger;

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

		public override GeoLocation Location {
			get { return base.Location;	}
			set {
				Log.LogMessage($"Received location in geowatcher {value}");
                if (IsBound) //&& (!base.Location?.Equals(value) ?? true))
				{
					base.Location = value;
					Mvx.Resolve<IMvxMessenger>().Publish(new LocationMessage(this, value));
					Log.LogMessage($"New location {value}");
				}
			}
		}

		public TouchGeolocationWatcher(IStoredSettingsService storedSettingsService, IMvxMessenger messenger)
		{
			_storedSettingsService = storedSettingsService;
			_messenger = messenger;
		}

		#region IGeoLocationWatcher implementation

		public virtual void StartGeolocationWatcher()
		{
			if (!IsBound) {
				StartLocationUpdates ();
				IsBound = true;				
			}	
		}

		public virtual void StopGeolocationWatcher ()
		{
			if (IsBound) {
				StopLocationUpdates ();
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
				StopLocationUpdates();
				LocationManager.DistanceFilter = distanceInterval;
				StartLocationUpdates();
			}
		}

		#endregion
	}
}