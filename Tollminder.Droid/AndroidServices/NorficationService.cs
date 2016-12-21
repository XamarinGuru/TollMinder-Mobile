using System;
using Android.App;
using Android.Content;

namespace Tollminder.Droid.AndroidServices
{
    public class PushNotificationService
    {
        public static void ShowNotification(Context context, Type activity, string tickerText, string contentText)
        {
            const int pendingIntentId = 0;
            const int notificationId = 0;

            // get activity
            Intent intentStart = new Intent(context, activity);
            //intentStart.SetAction(Intent.ActionMain);
            intentStart.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            //intentStart.AddCategory(Intent.CategoryLauncher);
            PendingIntent pendingIntent = PendingIntent.GetActivity(context, pendingIntentId, intentStart, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.OneShot);

            Notification.Builder builder = new Notification.Builder(context)
                                                                    .SetTicker("Tolminder")
                                                                    .SetContentIntent(pendingIntent)
                                                                    .SetContentTitle(tickerText)
                                                                    .SetContentText(contentText)
                                                                    .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                                                                    .SetPriority((int)NotificationPriority.High)
                                                                    .SetSmallIcon(Resource.Mipmap.ic_launcher);

            Notification notification = builder.Build();
            NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

            // publish the notification
            notificationManager.Notify(notificationId, notification);
        }
    }
}
