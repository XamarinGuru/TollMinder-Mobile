using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using MvvmCross.Droid.Support.V4;
using Plugin.Permissions;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.Adapters;
using Tollminder.Droid.Views.Fragments;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "SocialRegistrationView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class SocialRegistrationView : MvxFragmentActivity
    {
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
                SetContentView(Resource.Layout.social_registration_view);

            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo>
              {
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                    FragmentType = typeof(SmsConfirmationFragment),
                    ViewModel = ViewModel
                }
              };

            _viewPager = FindViewById<ViewPager>(Resource.Id.boardPager);
            _adapter = new MvxViewPagerFragmentAdapter(this, SupportFragmentManager, fragments);
            _viewPager.Adapter = _adapter;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
