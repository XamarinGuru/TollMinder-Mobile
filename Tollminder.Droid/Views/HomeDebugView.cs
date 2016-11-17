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


namespace Tollminder.Droid.Views
{
    [Activity(Label = "Home", Theme = "@style/AppTheme",LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class HomeDebugView : MvxActivity<HomeDebugViewModel>
    {
 		ScrollView _sv;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
            SetContentView(Resource.Layout.home_debug_view);

			_sv = FindViewById<ScrollView>(Resource.Id.sv);

			this.AddLinqBinding(ViewModel, vm => vm.LogText, (value) =>
			{
				_sv.FullScroll(Android.Views.FocusSearchDirection.Down);;
			});
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
    }
}