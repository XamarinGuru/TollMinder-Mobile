using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Tollminder.Core.Services;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.Inerfaces;
using Tollminder.Droid.Services.FacebookTools;
using Xamarin.Auth;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/app_name")]
namespace Tollminder.Droid.Views
{
    [Activity(Label = "LoginView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class LoginView : MvxFragmentActivity<LoginViewModel>
    {
        private static readonly string[] PERMISSION = { "publish_actions" };
        readonly string PENDING_ACTION_BUNDLE_KEY = "com.facebook.samples.hellofacebook:PendingAction";

        //ProfilePictureView profilePictureView;
        //TextView greeting;
        ICallbackManager callbackManager;
        ProfileTracker profileTracker;
        private bool goOnDestroy = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Initialize the SDK before executing any other operations
            //FacebookSdk.SdkInitialize(Application.Context);

             //create callback manager using CallbackManagerFactory
            //callbackManager = CallbackManagerFactory.Create();

            //LoginManager.Instance.RegisterCallback(callbackManager, new MyFacebookCallback<LoginResult>(this));

            SetContentView(Resource.Layout.login_view);

            //profileTracker = new MyProfileTracker(this);
            
            //profilePictureView = FindViewById<ProfilePictureView>(Resource.Id.profilePicture);
            //greeting = FindViewById<TextView>(Resource.Id.greeting);
        }


        protected override void OnResume()
        {
            base.OnResume();
            UpdateUI();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        public override void OnRestoreInstanceState(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnRestoreInstanceState(savedInstanceState, persistentState);
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        //protected override void OnDestroy()
        //{
            //base.OnDestroy();
        //    profileTracker.StopTracking();
        //}

        public void UpdateUI()
        {
            //bool enableButtons = AccessToken.CurrentAccessToken != null;

            //var profile = Profile.CurrentProfile;
            //if (enableButtons && profile != null)
            //{
            //    profilePictureView.ProfileId = profile.Id;
            //    greeting.Text = GetString(Resource.String.hello_user, profile.FirstName);
            //}
            //else
            //{
            //    profilePictureView.ProfileId = null;
            //    greeting.Text = null;
            //}
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            //callbackManager.OnActivityResult(requestCode, Convert.ToInt32(resultCode), data);
            (Mvx.Resolve<IFacebookLoginService>() as IDroidSocialLogin).OnActivityResult(requestCode, resultCode, data);
            //(Mvx.Resolve<IGPlusLoginService>()as IDroidSocialLogin).OnActivityResult(requestCode, resultCode, data);
        }
    }
}
