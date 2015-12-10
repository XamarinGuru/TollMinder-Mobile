using System;

namespace Tollminder.Core.Services
{
	public interface INotificationSender
	{
		void SendLocalNotification(string title, string message);
		void SendRemoteNotification();
	}
}

