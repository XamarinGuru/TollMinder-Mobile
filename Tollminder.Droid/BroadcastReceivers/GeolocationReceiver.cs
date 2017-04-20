using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Locations;
using Android.Runtime;
using MvvmCross.Droid.Services;
using MvvmCross.Platform;
using Tollminder.Core.Services.GeoData;

namespace Tollminder.Droid.BroadcastReceivers
{
    [RegisterAttribute("tollminder.droid.services.DroidGeolocationWatcher")]
    [BroadcastReceiver(Enabled = true, Exported = false)]
    [IntentFilter(new[] { "com.tollminder.GeolocationReciever" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class GeolocationReceiver : MvxBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if (LocationResult.HasResult(intent))
            {
                LocationResult locationResult = LocationResult.ExtractResult(intent);
                Location location = locationResult.LastLocation;
                if (location != null)
                {
                    Mvx.Resolve<IGeoLocationWatcher>().Location = location.GetGeolocationFromAndroidLocation();

                    //new Handler(context.MainLooper).Post(() =>
                    //{
                    //	Toast.MakeText(context, Mvx.Resolve<IGeoLocationWatcher>().Location?.ToString(), ToastLength.Short).Show();
                    //});
                }
            }
        }
    }
}