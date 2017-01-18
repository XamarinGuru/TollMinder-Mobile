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
    public class LicenseView : LifeCycleActivity<LicenseViewModel>
    {
        Spinner lastField;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.license_view);
            lastField = FindViewById<Spinner>(Resource.Id.license_spinner_vehicle_class);
            // hide keyboard when last editText lost focus
            lastField.FocusChange += (object sender, View.FocusChangeEventArgs e) => {
                if (!lastField.HasFocus)
                {
                    InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);

                    inputManager.HideSoftInputFromWindow(lastField.WindowToken, 0);
                }
            };    
        }
    }
}
