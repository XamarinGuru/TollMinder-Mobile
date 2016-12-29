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

namespace Tollminder.Droid.Views
{
    [Activity(Label = "License", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class LicenseView : MvxActivity<LicenseViewModel>
    {
        EditText lastEditText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.license_view);
            lastEditText = FindViewById<EditText>(Resource.Id.vehicle_class_editText);
            // hide keyboard when last editText lost focus
            lastEditText.FocusChange += (object sender, View.FocusChangeEventArgs e) => {
                if (!lastEditText.HasFocus)
                {
                    InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);

                    inputManager.HideSoftInputFromWindow(lastEditText.WindowToken, 0);
                }
            };    
        }
    }
}
