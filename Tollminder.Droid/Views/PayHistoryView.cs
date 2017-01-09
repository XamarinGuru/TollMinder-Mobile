using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.Views.Fragments;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "PaymentHistory", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class PaytHistoryView : MvxFragmentActivity<PayHistoryViewModel>
    {
        //ProfileButton calendar;
        TextView datePickerText;
        
        //string[] items;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.pay_history_view);
            datePickerText = FindViewById<TextView>(Resource.Id.date_from);
            datePickerText.Focusable = false;
            datePickerText.Click += delegate
            {
                // Create and show the dialog.
                var dialog = new CalendarDialogFragment();//.NewInstance(null);

                //Add fragment
                dialog.Show(SupportFragmentManager, "dialog");
            };
            //calendar = FindViewById<ProfileButton>(Resource.Id.date_from);
            //calendar.Click += delegate
            //{
            //    var dialog = new DatePickerDialogFragment(this, Convert.ToDateTime(datePickerText.Text), this);
            //    dialog.Show(FragmentManager, "date");
            //};
            //items = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            //ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
        }

    }
}
    