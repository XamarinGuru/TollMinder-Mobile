
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace Tollminder.Droid.Views
{

    [Activity(Label = "LoginView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true, LaunchMode = LaunchMode.SingleTask)]
    public class LoginView : BaseActivity<LoginViewModel>, GoogleApiClient.IOnConnectionFailedListener, View.IOnClickListener
    {
        Button btnLogin;

        #region Gmail fields
        const int googleSignInRequestCode = 9001;

        GoogleApiClient googleApiClient;
        SignInButton googleSignInButton;
        GoogleSignInOptions gso;
        #endregion

        #region Facebook fields
        const string facebookAppName = "TollMinder";
        const string facebookAppId = "355198514515820";
        ICallbackManager facebookCallbackManager;
        #endregion
        protected override int LayoutId
        {
            get
            {
                return Resource.Layout.login_view;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            #region Facebook
            FacebookSdk.ApplicationId = facebookAppId;
            FacebookSdk.ApplicationName = facebookAppName;
            FacebookSdk.SdkInitialize(Application.Context);
            #endregion

            base.OnCreate(bundle);

            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            btnLogin.Click += EmailLogin_Click;

            #region Gmail
            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail()
                .Build();
            googleApiClient = new GoogleApiClient.Builder(this)
                .EnableAutoManage(this, this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .Build();

            googleSignInButton = FindViewById<SignInButton>(Resource.Id.sign_in_button);
            googleSignInButton.SetSize(SignInButton.SizeStandard);
            googleSignInButton.SetOnClickListener(this);

            facebookCallbackManager = CallbackManagerFactory.Create();
            #endregion

            #region Facebook
            LoginManager.Instance.RegisterCallback(facebookCallbackManager, new MyFacebookCallback<LoginResult>(this));
            #endregion
        }

        void EmailLogin_Click(object sender, EventArgs e)
        {
            ViewModel.LoginCommand.Execute(ViewModel.EmailLoginData);
        }

        protected override void OnStart()
        {
            base.OnStart();
            googleApiClient.Connect();
        }

        protected override void OnStop()
        {
            base.OnStop();
            googleApiClient.Disconnect();
        }

        public void OnClick(View v)
        {
            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            StartActivityForResult(signInIntent, googleSignInRequestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == googleSignInRequestCode)
            {
                if (resultCode == Result.Ok)
                {
                    GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                    GoogleSignInAccount acct = result.SignInAccount;
                    if (acct != null)
                    {
                        Mvx.Trace($"Profile: {acct.DisplayName}, {acct.Email}, {acct.PhotoUrl}");
                        ViewModel.LoginCommand.Execute(new LoginData()
                        {
                            Email = acct.Email,
                            Name = acct.DisplayName,
                            Photo = acct.PhotoUrl.ToString(),
                            Source = AuthorizationType.GPlus
                        });
                    }
                }
            }
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
           Mvx.Trace("OnConnectionFailed:" + result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                btnLogin.Click -= EmailLogin_Click;
            }
            base.Dispose(disposing);
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
