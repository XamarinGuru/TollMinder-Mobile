
using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Org.Json;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.Inerfaces;
using Tollminder.Droid.Models;
using Tollminder.Droid.Services;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]

namespace Tollminder.Droid.Views
{

    [Activity(Label = "LoginView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class LoginView : BaseActivity<LoginViewModel>
    {
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.login_view;
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            (Mvx.Resolve<IFacebookLoginService>() as IDroidSocialLogin).OnActivityResult(requestCode, resultCode, data);
            (Mvx.Resolve<IGPlusLoginService>()as IDroidSocialLogin).OnActivityResult(requestCode, resultCode, data);
        }
    }
}
