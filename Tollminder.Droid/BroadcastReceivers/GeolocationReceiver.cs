using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.Runtime;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.BroadcastReceivers
{
	[RegisterAttribute("tollminder.droid.services.DroidGeolocationWatcher")]
	[BroadcastReceiver (Enabled = true, Exported = false)]
	[IntentFilter (new [] { "com.tollminder.GeolocationReciever" }, Priority = (int)IntentFilterPriority.HighPriority)]
	public class GeolocationReceiver : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			if (LocationResult.HasResult (intent)) {
				LocationResult locationResult = LocationResult.ExtractResult (intent);
				Location location = locationResult.LastLocation;
				if (location != null) {
					try
					{
						var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(Application.Context ?? context);
						setup.EnsureInitialized();
					}
					catch(Exception e)
					{
						//TODO: some mvvmcross initialization issies while deleting and starting app on thirt time
						//Cannot start primary - as state already InitializingSecondary
						Mvx.Trace(e.Message + e.StackTrace);
					}

					Mvx.Resolve<IGeoLocationWatcher>().Location = location.GetGeolocationFromAndroidLocation();
				}
			}
		}
	}
}