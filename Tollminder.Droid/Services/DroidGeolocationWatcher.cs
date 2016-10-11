using System;
using System.Collections.Generic;
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

        List<MvxSubscriptionToken> _tokens = new List<MvxSubscriptionToken>();

		#region IGeoLocationWatcher implementation
		public DroidGeolocationWatcher (IStoredSettingsService storedSettingsService, IMvxMessenger messenger)
		{
			_storedSettingsService = storedSettingsService;
			_messenger = messenger;
			ServiceIntent = new Intent (ApplicationContext, typeof (GeolocationService));

            _tokens.Add(_messenger.SubscribeOnThreadPoolThread<MotionMessage>(x =>
            {
                Log.LogMessage($"[DroidGeolocationWatcher] receive new motion type {x.Data}");
                switch (x.Data)
                {
                    case MotionType.Automotive:
                    case MotionType.Running:
                    case MotionType.Walking:
                        if ((_storedSettingsService.SleepGPSDateTime == DateTime.MinValue || _storedSettingsService.SleepGPSDateTime < DateTime.Now)
                            && !IsBound)
                        {
                            Log.LogMessage($"[DroidGeolocationWatcher] Start geolocating because we are not still");
                            StartGeolocationWatcher();
                        }
                        break;
                    case MotionType.Still:
                        if (IsBound)
                        {
                            Log.LogMessage($"[DroidGeolocationWatcher] Stop geolocating because we are still");
                            StopGeolocationWatcher();
                        }
                        break;
                }
            }));
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
            Log.LogMessage("StartGeolocationWatcher start");
			if (!IsBound && ApplicationContext.IsGooglePlayServicesInstalled ()) {
				Log.LogMessage("StartGeolocationWatcher success");
				Start ();
				IsBound = true;
			}
		}

		public virtual void StopGeolocationWatcher ()
		{
            Log.LogMessage("StopGeolocationWatcher init");
			if (IsBound) {
				Stop ();
				Log.LogMessage("StopGeolocationWatcher success");
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