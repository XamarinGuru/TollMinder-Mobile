using System;
using Android.App;

namespace Tollminder.Droid.Services
{
    public class CalendarService
    {
        public DateTime date;
        public CalendarService(AlertDialog dialog, DateTime date)
        {
            this.date = date;
            CloseDialog(dialog);
        }

        void CloseDialog(AlertDialog dialog)
        {
            dialog.Dismiss();
        }
    }
}
