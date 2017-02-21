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
using Syncfusion.SfDataGrid;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.Views.Fragments;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "PaymentHistory", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class PaytHistoryView : MvxFragmentActivity<PayHistoryViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.pay_history_view);

            var calendarRadioCroups = FindViewById<RadioGroup>(Resource.Id.calendarRadioCroups);
            calendarRadioCroups.Check(Resource.Id.current);
        }
    }
}
