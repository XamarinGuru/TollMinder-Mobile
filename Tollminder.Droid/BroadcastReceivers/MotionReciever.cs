using MvvmCross.Platform;
using Tollminder.Core.Services;
using Android.Content;
using Android.Gms.Location;
using Android.App;
using MvvmCross.Droid.Services;

namespace Tollminder.Droid.BroadcastReceivers
{
	[BroadcastReceiver (Enabled = true, Exported = false)]
	[IntentFilter (new [] { "com.tollminder.MotionReciever" })]
	public class MotionReciever : MvxBroadcastReceiver
	{
		public override void OnReceive (Context context, Intent intent)
		{
            base.OnReceive(context, intent);

			if (ActivityRecognitionResult.HasResult (intent)) {
				ActivityRecognitionResult result = ActivityRecognitionResult.ExtractResult (intent);
				if (result != null) 
                {
					Mvx.Resolve<IMotionActivity> ().MotionType = result.MostProbableActivity.GetMotionType ();
				}                
			}
		}
	}
}