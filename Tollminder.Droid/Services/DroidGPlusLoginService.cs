using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Support.V4.App;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.Inerfaces;

namespace Tollminder.Droid.Services
{
    public class DroidGPlusLoginService : Java.Lang.Object, IGPlusLoginService, GoogleApiClient.IOnConnectionFailedListener, IDroidSocialLogin
    {
        const int googleSignInRequestCode = 9001;
        GoogleApiClient googleApiClient;
        SignInButton googleSignInButton;
        GoogleSignInOptions gso;

        TaskCompletionSource<PersonData> _gPlusTask;

        public DroidGPlusLoginService()
        {
        }

        public void Initialize()
        {
            Initialize(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity as FragmentActivity);
        }

        public void Initialize(FragmentActivity activity)
        {
            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
               .RequestEmail()
               .Build();
            googleApiClient = new GoogleApiClient.Builder(Application.Context)
               .EnableAutoManage(activity, this)
               .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
               .Build();

            googleSignInButton = activity.FindViewById<SignInButton>(Resource.Id.sign_in_button);
            googleSignInButton.SetSize(SignInButton.SizeStandard);
            googleApiClient.Connect();
        }

        public Task<PersonData> Login()
        {
            _gPlusTask = new TaskCompletionSource<PersonData>();

            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivityForResult(signInIntent, googleSignInRequestCode);

            googleSignInButton.PerformClick();
            return _gPlusTask.Task;
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            Mvx.Trace("OnConnectionFailed:" + result);
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == googleSignInRequestCode)
            {
                if (resultCode == Result.Ok)
                {
                    GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                    GoogleSignInAccount acct = result.SignInAccount;
                    if (acct != null)
                    {
                        Mvx.Trace($"Profile: {acct.DisplayName}, {acct.Email}, {acct.PhotoUrl}");
                        googleApiClient.Disconnect();
                        _gPlusTask.TrySetResult(new PersonData()
                        {
                            Email = acct.Email,
                            Name = acct.DisplayName,
                            Photo = acct.PhotoUrl?.ToString(),
                            Source = AuthorizationType.GPlus
                        });
                    }
                }
            }
        }
    }
}
