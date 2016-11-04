
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/app_id")]
[assembly: MetaData("com.facebook.sdk.ApplicationName", Value = "@string/facebook_app_name")]

namespace Tollminder.Droid.Views
{
    
    [Activity(Label = "LoginView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class LoginView : BaseActivity<LoginViewModel>, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener, View.IOnClickListener
    {
        GoogleApiClient mGoogleApiClient;

        const int RC_SIGN_IN = 9001;
        private static readonly string[] PERMISSION = { "email" };

        SignInButton _googleSignIn;
        ICallbackManager callbackManager;

        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.login_view;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            FacebookSdk.SdkInitialize(Application.Context);

            base.OnCreate(bundle);
            mGoogleApiClient = new GoogleApiClient.Builder(this)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(PlusClass.API)
                .AddScope(new Scope(Scopes.Profile))
                .Build();
            _googleSignIn = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            _googleSignIn.SetSize(SignInButton.SizeStandard);

            _googleSignIn.SetOnClickListener(this);

            callbackManager = CallbackManagerFactory.Create();

            LoginManager.Instance.RegisterCallback(callbackManager, new MyFacebookCallback<LoginResult>(this));
        }

        public void OnConnected(Bundle connectionHint)
        {
           
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            
        }

        public void OnConnectionSuspended(int cause)
        {
           
        }

        private void UpdateUI()
        {
        }

        private void HandlePendingAction()
        {
            
        }

        public void OnClick(View v)
        {
            mGoogleApiClient.Connect();
        }
    }

    class MyFacebookCallback<LoginResult> : Java.Lang.Object, IFacebookCallback where LoginResult : Java.Lang.Object
    {

        readonly LoginView owner;

        public MyFacebookCallback(LoginView owner)
        {
            this.owner = owner;
        }

        public void OnSuccess(Java.Lang.Object obj)
        {
           
        }

        public void OnCancel()
        {
            
        }

        public void OnError(FacebookException fbException)
        {
            
        }
    }
}
