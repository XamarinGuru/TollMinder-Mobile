
using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Tollminder.Core.ViewModels;
using Plugin.Permissions;
using Android.Content.PM;
using Android.Widget;
using Tollminder.Core.Helpers;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using System;
using Tollminder.Droid.AndroidServices;
using Android.Content;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Home", Theme = "@style/AppTheme",LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class HomeView : MvxActivity<HomeViewModel>
    {
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.home_view);
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

        protected override void OnPause()
        {
            base.OnPause();Intent intentStart = new Intent(this, typeof(HomeView));
            PushNotificationService.ShowNotification(this, this.GetType(), "Tollminder - still working", "Press to open.");
        }
    }
}