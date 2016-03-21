using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Services
{
	[BroadcastReceiver (Enabled = true, Exported = false)]
	[IntentFilter (new [] { "com.tollminder.GeolocationReciever" })]
	public class DroidGeolocationWatcher : DroidServiceStarter, IGeoLocationWatcher
	{	
		#region IGeoLocationWatcher implementation
		public DroidGeolocationWatcher () 
		{
			ServiceIntent = new Intent (ApplicationContext, typeof (GeolocationService));
		}

		public bool IsBound { get; private set; } = false;

		GeoLocation _location;
		public virtual GeoLocation Location {
			get {
				return _location;
			}
			set {
				_location = value;
				Mvx.Resolve<IMvxMessenger> ().Publish (new LocationMessage (this, Location));
				#if DEBUG
				Log.LogMessage (value.ToString ());
				#endif
			}
		}

		public virtual void StartGeolocationWatcher ()
		{
			if (!IsBound && ApplicationContext.IsGooglePlayServicesInstalled()) {
				Start ();
				IsBound = true;				
			}
		}

		public virtual void StopGeolocationWatcher ()
		{
			if (IsBound ) {				
				Stop ();
				IsBound = false;
			}
		}

		public virtual void StartUpdatingHighAccuracyLocation()
		{			
			if (IsBound) {
				Stop ();
				ServiceIntent.PutExtra ("interval", 4000);
				Start ();
			}
		}

		public virtual void StopUpdatingHighAccuracyLocation()
		{			
			if (IsBound) {
				Stop ();
				ServiceIntent.PutExtra ("interval", 20000);
				Start ();
			}
		}

		public override void OnReceive (Context context, Intent intent)
		{
			base.OnReceive (context, intent);
			if (LocationResult.HasResult (intent)) {
				LocationResult locationResult = LocationResult.ExtractResult (intent);
				Location location = locationResult.LastLocation;
				if (location != null) {
					Location = location.GetGeolocationFromAndroidLocation ();
				}
			}
		}
		#endregion
	}
}