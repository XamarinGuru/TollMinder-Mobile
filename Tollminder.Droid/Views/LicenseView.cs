﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Tollminder.Core.ViewModels;
using Tollminder.Core.ViewModels.Vehicles;
using Tollminder.Droid.AndroidServices;
using MvvmCross.Droid.Views;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "License", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class LicenseView : MvxActivity<LicenseViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.license_view);

            // hide keyboard when last field lost focus
            FieldsService.LostFocusFromField(Resource.Id.license_spinner_vehicle_class);
        }
    }
}
