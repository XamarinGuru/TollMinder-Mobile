using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : TouchLocation, IGeoLocationWatcher
	{	
		public bool IsBound { get; private set; } = false;

		public override GeoLocation Location {
			get { return base.Location;	}
			set {
				base.Location = value;
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
			LocationManager.DistanceFilter = 30;
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