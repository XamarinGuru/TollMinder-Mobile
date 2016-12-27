using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Profile", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class ProfileView : MvxActivity<ProfileViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.profile_view);
        }
    }
}
