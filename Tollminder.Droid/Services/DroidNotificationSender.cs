using System;
using Tollminder.Core.Services;
using Android.Support.V7.App;
using Android.Graphics;
using Android.App;
using Android.Content;
using Tollminder.Droid.Views;
using Android.Content.Res;

namespace Tollminder.Droid.Services
{
	public class DroidNotificationSender : INotificationSender
	{
		#region INotificationSender implementation
		public void SendLocalNotification (string title, string message)
		{
			var notificationIntent = new Intent (Application.Context, typeof(HomeView));

			var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create (Application.Context);
			stackBuilder.AddParentStack (Java.Lang.Class.FromType (typeof(HomeView)));
			stackBuilder.AddNextIntent (notificationIntent);

			var notificationPendingIntent =	stackBuilder.GetPendingIntent (0, (int)PendingIntentFlags.UpdateCurrent);

			var builder = new NotificationCompat.Builder (Application.Context);
			builder.SetSmallIcon (Resource.Mipmap.Icon)
				.SetLargeIcon (BitmapFactory.DecodeResource (Application.Context.Resources, Resource.Mipmap.Icon))
				.SetColor (Color.Red)
				.SetContentTitle (title)
				.SetContentText (message)
				.SetContentIntent (notificationPendingIntent);

			builder.SetAutoCancel (true);

			var mNotificationManager = (NotificationManager)Application.Context.GetSystemService (Context.NotificationService);
			mNotificationManager.Notify (0, builder.Build ());
		}
		public void SendRemoteNotification ()
		{
			throw new NotImplementedException ();
		}
		#endregion
		
	}
}

