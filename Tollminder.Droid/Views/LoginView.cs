
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
using Java.Lang;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Org.Json;
using Tollminder.Core.Models;
using Tollminder.Core.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.AppEvents;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;

[assembly: Permission(Name = Android.Manifest.Permission.Internet)]
[assembly: Permission(Name = Android.Manifest.Permission.WriteExternalStorage)]

namespace Tollminder.Droid.Views
{

    [Activity(Label = "LoginView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true, LaunchMode = LaunchMode.SingleTask)]
    public class LoginView : BaseActivity<LoginViewModel>, GoogleApiClient.IOnConnectionFailedListener, View.IOnClickListener, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
    {
        Button btnLogin;

        #region G+ fields
        const int googleSignInRequestCode = 9001;
        GoogleApiClient googleApiClient;
        SignInButton googleSignInButton;
        GoogleSignInOptions gso;
        #endregion

        #region Facebook fields
        const int facebookSignInRequestCode = 9002;
        const string facebookAppName = "TollMinder";
        const string facebookAppId = "194561500997971";
        ICallbackManager facebookCallbackManager;
        LoginButton loginButton;
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

            #region G+
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
            #endregion

            #region Facebook
            loginButton = FindViewById<LoginButton>(Resource.Id.login_button);
            LoginManager.Instance.LogInWithReadPermissions(this, new List<string> { "public_profile", "email" });
            facebookCallbackManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(facebookCallbackManager, this);
            LoginManager.Instance.LogOut();
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

        #region G+
        public void OnClick(View v)
        {
            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            StartActivityForResult(signInIntent, googleSignInRequestCode);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            facebookCallbackManager.OnActivityResult(requestCode, (int)resultCode, data);

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
        #endregion

        #region Facebook
        public void OnCancel()
        {
           
        }

        public void OnError(FacebookException error)
        {
           
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            ViewModel.IsBusy = true;
            try
            {
                GraphRequest request = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);

                Bundle parameters = new Bundle();
                parameters.PutString("fields", "id,name,email");
                request.Parameters = parameters;
                request.ExecuteAsync();
                LoginManager.Instance.LogOut();
            }
            catch(System.Exception e)
            {
                Mvx.Trace(e.Message + e.StackTrace);
            }
            finally
            {
                ViewModel.IsBusy = false;
            }
        }

        public void OnCompleted(JSONObject json, GraphResponse response)
        {
            FacebookAccountResult acct = JsonConvert.DeserializeObject<FacebookAccountResult>(json.ToString());

            if (acct != null)
            {
                Mvx.Trace($"Profile: {acct.Name}, {acct.Email}, {acct.PhotoUrl}");
                ViewModel.LoginCommand.Execute(new LoginData()
                {
                    Email = acct.Email,
                    Name = acct.Name,
                    Photo = acct.PhotoUrl,
                    Source = AuthorizationType.Facebook
                });
            }
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                btnLogin.Click -= EmailLogin_Click;
            }
            base.Dispose(disposing);
        }
    }

    class FacebookAccountResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string PhotoUrl
        {
            get
            {
                return $"https://graph.facebook.com/{Id}/picture?type=large";
            }
        }
    }
}
