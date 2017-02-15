using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using Plugin.Permissions;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.Adapters;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Views.Fragments;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "RegistrationView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class RegistrationView : LifeCycleActivity<RegistrationViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.registration_view);

            // hide keyboard when last editText lost focus
            FieldsService.LostFocusFromField(Resource.Id.phoneNumber_editText);
        }
    }
}
