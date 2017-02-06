using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Tollminder.Core.Services;
using Tollminder.Core.ViewModels;

namespace Tollminder.Droid.Views.Fragments
{
    public class CalendarDialog : ICalendarDialog
    {
        ImageButton backToPayHistory;
        CalendarView calendarView;
        AlertDialog dialog;
        CalendarViewModel model = new CalendarViewModel();
        View view;

        //public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    //backToPayHistory = view.FindViewById<ImageButton>(Resource.Id.calendar_btn_back_to_payhistory);
           
        //    //var set = this.CreateBindingSet<CalendarDialog, PayHistoryViewModel>();
        //    //set.Bind(backToPayHistory).To(vm => vm.BackToPayHistoryCommand);
        //    //set.Apply(); 
        //    //return base.OnCreateView(inflater, container, savedInstanceState);
        //    var ignored = base.OnCreateView(inflater, container, savedInstanceState);
        //    return this.BindingInflate(Resource.Layout.ChildView, null);
        //}

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
            var result = new TaskCompletionSource<DateTime>();
            System.Diagnostics.Debug.WriteLine(calendarView.Date);

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