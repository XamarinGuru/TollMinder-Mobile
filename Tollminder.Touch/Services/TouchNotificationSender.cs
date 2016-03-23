using System;
using Tollminder.Core.Services;
using UIKit;

namespace Tollminder.Touch.Services
{
	public class TouchNotificationSender : INotificationSender
	{
		#region INotificationSender implementation

		public void SendLocalNotification (string title, string message)
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				var notification = new UILocalNotification ();
				notification.AlertTitle = title;
				notification.AlertBody = message;
				notification.SoundName = "Default";
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

