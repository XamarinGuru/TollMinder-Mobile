using System;
using System.Threading.Tasks;
using Android.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Tollminder.Core.Services;
using Tollminder.Core.ViewModels;
using Tollminder.Core.ViewModels.Payments;

namespace Tollminder.Droid.Views.Fragments
{
    public class CalendarDialog : ICalendarDialog
    {
        ImageButton backToPayHistory;
        CalendarView calendarView;
        AlertDialog dialog;
        CalendarViewModel model = new CalendarViewModel();
        View view;

        protected Activity CurrentActivity
        {
            get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
        }

        public Task<DateTime> ShowDialog(DateTime currentValue)
        {
            view = CurrentActivity.LayoutInflater.Inflate(Resource.Layout.calendar_fragment, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(CurrentActivity);
            builder.SetView(view);
            builder.Create();
            dialog = builder.Show();

            calendarView = view.FindViewById<CalendarView>(Resource.Id.calendarView); 
            backToPayHistory = view.FindViewById<ImageButton>(Resource.Id.calendar_btn_back_to_payhistory);
            var result = new TaskCompletionSource<DateTime>();
            var unixTime = currentValue.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            calendarView.Date = (long)unixTime.TotalMilliseconds;

            calendarView.DateChange += (sim, args) =>
            {
                var year = args.Year;
                var month = args.Month + 1;
                var dayOfMont = args.DayOfMonth;

                var date = new DateTime(year, month, dayOfMont);
                if (result != null)
                {
                    result.SetResult(date);
                }
                dialog.Dismiss();
            };

            backToPayHistory.Click+= (sender, e) => 
            {
                dialog.Dismiss();
            };

            model.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "DialogVisible" && !model.DialogVisible)
                {
                    dialog.Dismiss();
                }
            };

            return result.Task;
        }
    }
}