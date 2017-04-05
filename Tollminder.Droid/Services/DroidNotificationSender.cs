﻿using System;
using Android.App;
using Tollminder.Droid.Views;
using Tollminder.Droid.AndroidServices;
using Tollminder.Core.Services.Notifications;

namespace Tollminder.Droid.Services
{
    public class DroidNotificationSender : INotificationSender
    {
        #region INotificationSender implementation
        public void SendLocalNotification(string title, string message)
        {
            //var notificationIntent = new Intent (Application.Context, typeof(HomeDebugView));

            //var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create (Application.Context);
            //stackBuilder.AddParentStack (Java.Lang.Class.FromType (typeof(HomeDebugView)));
            //stackBuilder.AddNextIntent (notificationIntent);

            //var notificationPendingIntent =	stackBuilder.GetPendingIntent (0, (int)PendingIntentFlags.UpdateCurrent);

            //var builder = new NotificationCompat.Builder (Application.Context);
            //         builder.SetSmallIcon (Resource.Mipmap.icon)
            //	.SetLargeIcon (BitmapFactory.DecodeResource (Application.Context.Resources, Resource.Mipmap.icon))
            //	.SetColor (Color.Red)
            //	.SetContentTitle (title)
            //	.SetContentText (message)
            //	.SetSound (RingtoneManager.GetDefaultUri (RingtoneType.Notification))
            //	.SetContentIntent (notificationPendingIntent);

            //builder.SetAutoCancel (true);

            //var mNotificationManager = (NotificationManager)Application.Context.GetSystemService (Context.NotificationService);
            //mNotificationManager.Notify (0, builder.Build ());
            PushNotificationService.ShowNotification(Application.Context, typeof(HomeView), title, message);

        }
        public void SendRemoteNotification()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}

