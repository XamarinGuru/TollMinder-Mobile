using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using MvvmCross.Droid.Services;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Views;

namespace Tollminder.Droid.BroadcastReceivers
{
    [RegisterAttribute("tollminder.droid.BroadcastReceivers.RebootReceiver")]
    [BroadcastReceiver(Enabled = true, Exported = false)]
    [IntentFilter(new[] { Intent.ActionBootCompleted }, Categories = new[] { "android.intent.category.HOME" }, Priority = (int)IntentFilterPriority.HighPriority)]
    public class RebootReceiver : MvxBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            base.OnReceive(context, intent);

            if ((intent.Action != null) && (intent.Action == Android.Content.Intent.ActionBootCompleted))
            {
                PushNotificationService.ShowNotification(context, typeof(SplashScreen), "Tollminder - open for tracking", "Tollminder is not started");
                //Toast.MakeText(context, “Received intent!”, ToastLength.Short).Show();

                //if ((intent.Action != null) &&
                //(intent.Action ==
                //Android.Content.Intent.ActionBootCompleted))
                //{ // Start the service or activity
                //  //context.ApplicationContext.StartService(new Intent(context, typeof(MainActivity)));

                //    Android.Content.Intent start = new Android.Content.Intent(context, typeof(HomeDebugView));

                //    // my activity name is MainActivity replace it with yours
                //    start.AddFlags(ActivityFlags.NewTask);
                //    context.ApplicationContext.StartActivity(start);
                //}
            }
                //new Handler(context.MainLooper).Post(() =>
                //            {
                //                Toast.MakeText(context, "Tollminder not started.", ToastLength.Short).Show();
                //            });
        }
    }

}
