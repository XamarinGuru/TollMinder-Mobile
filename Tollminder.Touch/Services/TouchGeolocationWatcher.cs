using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;
using System;
using System.Reactive.Linq;
using UIKit;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : TouchLocation, IGeoLocationWatcher
	{	
		public bool IsBound { get; private set; } = false;

		public override GeoLocation Location {
			get { return base.Location;	}
			set {
				base.Location = value;
				#if DEBUG
				Log.LogMessage ("MESSAGE WITH LOCATION PUBLISH");
				#endif
				Mvx.Resolve<IMvxMessenger> ().Publish (new LocationMessage (this, Location));
				#if DEBUG
				Log.LogMessage (value.ToString ());
				#endif
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
			StoptLocationUpdates ();
			LocationManager.DistanceFilter = 10;
			StartLocationUpdates ();
		}

		public virtual void StopUpdatingHighAccuracyLocation ()
		{
			StoptLocationUpdates ();
			LocationManager.DistanceFilter = 400;
			StartLocationUpdates ();
		}
		#endregion
	}
}