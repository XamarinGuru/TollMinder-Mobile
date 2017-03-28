using System;

namespace Tollminder.Core.Services.Notifications
{
    public interface INotificationSender
    {
        void SendLocalNotification(string title, string message);
        void SendRemoteNotification();
    }
}

