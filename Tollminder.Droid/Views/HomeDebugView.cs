using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels;
using Plugin.Permissions;
using Android.Content.PM;
using Android.Widget;
using Tollminder.Core.Helpers;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Home", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
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
                _sv.FullScroll(Android.Views.FocusSearchDirection.Down);
            });
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnPause()
        {
            base.OnPause();
            PushNotificationService.ShowNotification(this, this.GetType(), "Tollminder - still working", "Press to open.");
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}