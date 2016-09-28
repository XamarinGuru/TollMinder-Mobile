using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;
using MvvmCross.Plugins.Messenger;
using Android.Content;
using Android.Gms.Location;
using Android.App;
using MvvmCross.Droid.Platform;
using System;

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
				if (result != null) {
					try
					{
						var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(Application.Context ?? context);
						setup.EnsureInitialized();
					}
					catch (Exception e)
					{
						//TODO: some mvvmcross initialization issies while deleting and starting app on thirt time
						//Cannot start primary - as state already InitializingSecondary
						Mvx.Trace(e.Message + e.StackTrace);
					}

					Mvx.Resolve<IMotionActivity> ().MotionType = result.MostProbableActivity.GetMotionType ();
				}                
			}
		}
	}
}