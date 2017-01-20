using Android.App;
using Android.OS;
using Tollminder.Core.ViewModels;
using Plugin.Permissions;
using Android.Content.PM;
using Android.Support.V4.View;
using Tollminder.Droid.Adapters;
using System.Collections.Generic;
using Tollminder.Droid.Views.Fragments;
using MvvmCross.Droid.Support.V4;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Home", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class HomeView : MvxFragmentActivity
    {
        private ViewPager _viewPager;
        private MvxViewPagerFragmentAdapter _adapter;

        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.home_view);

            var fragments = new List<MvxViewPagerFragmentAdapter.FragmentInfo>
              {
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(ButtonContainerFragment),
                  ViewModel = ViewModel
                },
                new MvxViewPagerFragmentAdapter.FragmentInfo
                {
                  FragmentType = typeof(RoadInformationFragment),
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

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //PushNotificationService.ShowNotification(this, this.GetType(), "Tollminder - still working", "Press to open.");
        }
    }
}