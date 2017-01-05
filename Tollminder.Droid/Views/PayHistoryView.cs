using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.Controls;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "PaymentHistory", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class PaytHistoryView : MvxActivity<PayHistoryViewModel>
    {
        ProfileButton calendar;
        
        string[] items;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.pay_history_view);
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
    