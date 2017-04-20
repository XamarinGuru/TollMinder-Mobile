using MvvmCross.Platform;
using Android.Content;
using Android.Gms.Location;
using Android.App;
using MvvmCross.Droid.Services;
using Tollminder.Core.Services.RoadsProcessing;

namespace Tollminder.Droid.BroadcastReceivers
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    [IntentFilter(new[] { "com.tollminder.MotionReciever" })]
    public class MotionReciever : MvxBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(Application.Context, intent);

            if (ActivityRecognitionResult.HasResult(intent))
            {
                ActivityRecognitionResult result = ActivityRecognitionResult.ExtractResult(intent);
                if (result != null)
                {
                    var motion = result.MostProbableActivity.GetMotionType();
                    if (motion != Core.Models.MotionType.Unknown)
                        Mvx.Resolve<IMotionActivity>().MotionType = motion;

                    //new Handler(context.MainLooper).Post(() =>
                    //{
                    //    Toast.MakeText(context, motion.ToString(), ToastLength.Short).Show();
                    //});
                }
            }
        }
    }
}