using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;
using System;
using System.Timers;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : TouchLocation, IGeoLocationWatcher
	{	
		public static int _distanceIntervalDefault = 400;
		public bool IsBound { get; private set; } = false;

		public override GeoLocation Location {
			get { return base.Location;	}
			set {
				if (IsBound && (!base.Location?.Equals(value) ?? true))
				{
					base.Location = value;
					Mvx.Resolve<IMvxMessenger>().Publish(new LocationMessage(this, value));
					Log.LogMessage($"New location {value}");
				}
			}
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
				StoptLocationUpdates ();
				IsBound = false;
			}
		}

		public virtual void StartUpdatingHighAccuracyLocation ()
		{
			UpdateAccuracyLocation(20);
		}

		public virtual void StopUpdatingHighAccuracyLocation ()
		{
			UpdateAccuracyLocation(_distanceIntervalDefault);
		}

		void UpdateAccuracyLocation(int distanceInterval)
		{
			if (IsBound)
			{
				StoptLocationUpdates();
				LocationManager.DistanceFilter = distanceInterval;
				StartLocationUpdates();
			}
		}

		#endregion
	}
}