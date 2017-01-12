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
using Android.Graphics;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Util;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Home", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class HomeView : MvxActivity<HomeViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Display display = WindowManager.DefaultDisplay;
            //DisplayMetrics outMetrics = new DisplayMetrics();
            //display.GetMetrics(outMetrics);

            DisplayMetrics displayMetrics = Resources.DisplayMetrics;
            float dpHeight = displayMetrics.HeightPixels / displayMetrics.Density;
            float dpWidth = displayMetrics.WidthPixels / displayMetrics.Density;
            Point size = new Point();
            display.GetSize(size);
            int width = size.X;
            int height = size.Y;
            SetContentView(Resource.Layout.home_view);
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