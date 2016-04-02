using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.Runtime;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Services
{
	[RegisterAttribute("tollminder.droid.services.DroidGeolocationWatcher")]
	[BroadcastReceiver (Enabled = true, Exported = false)]
	[IntentFilter (new [] { "com.tollminder.GeolocationReciever" }, Priority = (int)IntentFilterPriority.HighPriority)]
	public class DroidGeolocationWatcher : DroidServiceStarter, IGeoLocationWatcher
	{
		static int asdfasdf = 0;

		#region IGeoLocationWatcher implementation
		public DroidGeolocationWatcher () 
		{
			asdfasdf++;
			ServiceIntent = new Intent (ApplicationContext, typeof (GeolocationService));
		}

		public bool IsBound { get; private set; } = false;

		private static GeoLocation _location;
		public virtual GeoLocation Location {
			get {
				return _location;
			}
			set {
				_location = value;
				Mvx.Resolve<IMvxMessenger> ().Publish (new LocationMessage (this, value));
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
				ServiceIntent.PutExtra ("interval", 20);
				Start ();
			}
		}

		public virtual void StopUpdatingHighAccuracyLocation()
		{			
			if (IsBound) {
				Stop ();
				ServiceIntent.PutExtra ("interval", 400);
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