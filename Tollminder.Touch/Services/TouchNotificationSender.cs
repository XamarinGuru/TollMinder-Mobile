using System;
using Tollminder.Core.Services;
using UIKit;

namespace Tollminder.Touch.Services
{
	public class TouchNotificationSender : INotificationSender
	{
        UILocalNotification notification;

		#region INotificationSender implementation

		public void SendLocalNotification (string title, string message)
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				notification = new UILocalNotification ();
				notification.AlertTitle = title;
				notification.AlertBody = message;
				notification.SoundName = UILocalNotification.DefaultSoundName;
                //notification.AlertAction = UILocalNotification.
                UIApplication.SharedApplication.CancelLocalNotification(notification);
                UIApplication.SharedApplication.PresentLocalNotificationNow (notification);
			});
		}

		public void SendRemoteNotification ()
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

