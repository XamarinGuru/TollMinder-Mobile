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
using Tollminder.Droid.Views.Fragments;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "SocialRegistrationView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class RegistrationView : MvxFragmentActivity
    {
        EditText lastEditText;
        private ViewPager _viewPager;
        private MvxViewPagerFragmentAdapter _adapter;

        public new RegistrationViewModel ViewModel
        {
            get { return (RegistrationViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.registration_view);

            lastEditText = FindViewById<EditText>(Resource.Id.phoneNumber_editText);
            // hide keyboard when last editText lost focus
            lastEditText.FocusChange += (object sender, View.FocusChangeEventArgs e) =>
            {
                if (!lastEditText.HasFocus)
                {
                    InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);

                    inputManager.HideSoftInputFromWindow(lastEditText.WindowToken, 0);
                }
            };

            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo>
              {
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                    FragmentType = typeof(SmsConfirmationFragment),
                    ViewModel = ViewModel
                }
              };

            _viewPager = FindViewById<ViewPager>(Resource.Id.sms_confirmation_container);
            _adapter = new MvxViewPagerFragmentAdapter(this, SupportFragmentManager, fragments);
            _viewPager.Adapter = _adapter;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
