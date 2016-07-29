using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;
using MvvmCross.Plugins.Messenger;
using Android.Content;
using Android.Gms.Location;
using Android.App;
using MvvmCross.Droid.Platform;

namespace Tollminder.Droid.BroadcastReceivers
{
	[BroadcastReceiver (Enabled = true, Exported = false)]
	[IntentFilter (new [] { "com.tollminder.MotionReciever" })]
	public class MotionReciever : BroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
			if (ActivityRecognitionResult.HasResult (intent)) {
				ActivityRecognitionResult result = ActivityRecognitionResult.ExtractResult (intent);
				if (!Mvx.CanResolve<IMotionActivity> ()) {
					var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable (context);
					setup.EnsureInitialized ();
				}
				Mvx.Resolve<IMotionActivity> ().MotionType = result.MostProbableActivity.GetMotionType ();
			}
		}
	}
}