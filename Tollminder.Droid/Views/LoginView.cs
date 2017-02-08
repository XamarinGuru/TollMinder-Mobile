using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Support.V4;
using Tollminder.Core.ViewModels;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]
namespace Tollminder.Droid.Views
{
    [Activity(Label = "LoginView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class LoginView : MvxFragmentActivity<LoginViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.login_view);
        }
    }
}
