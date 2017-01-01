using System;
using Foundation;
using UIKit;

namespace Tollminder.Touch.Services
{
    public class TouchSheduleNotification
    {
        public static void SheduleNotification()
        {
            if(UIApplication.SharedApplication.ScheduledLocalNotifications.Rank == 0)
            {
                var notification = new UILocalNotification();

                // configure the alert
                notification.AlertAction = "HEy man!!!!!!";
                notification.AlertBody = "Your 10 second alert has fired!";

                // modify the badge
                notification.ApplicationIconBadgeNumber = 1;
                notification.SoundName = UILocalNotification.DefaultSoundName;

                // set the fire date (the date time in which it will fire)
                notification.FireDate = NSDate.FromTimeIntervalSinceNow(10);
                //notification.FireDate = new Foundation.NSDate();
                notification.RepeatInterval = Foundation.NSCalendarUnit.Day;
                UIApplication.SharedApplication.ScheduleLocalNotification(notification);
            }
        }
    }
}
