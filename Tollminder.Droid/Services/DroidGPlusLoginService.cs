using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Plus;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.Inerfaces;

namespace Tollminder.Droid.Services
{
    public class DroidGPlusLoginService : Java.Lang.Object, IGPlusLoginService, GoogleApiClient.IOnConnectionFailedListener, GoogleApiClient.IConnectionCallbacks, IDroidSocialLogin
    {
        const int googleSignInRequestCode = 9001;

        GoogleApiClient googleApiClient;
        GoogleSignInOptions gso;
        ConnectionResult connectionResult;

        private bool intentInProgress;
        private bool signInClicked;
        private bool infoPopulated;

        TaskCompletionSource<SocialData> _gPlusTask;

        public DroidGPlusLoginService()
        {
        }

        public void Initialize()
        {
            Initialize(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity as FragmentActivity);
        }

        public void Initialize(FragmentActivity activity)
        {
            googleApiClient = new GoogleApiClient.Builder(activity)
                .AddConnectionCallbacks(this)
                .AddOnConnectionFailedListener(this)
                .AddApi(PlusClass.API)
                                                 .AddScope(PlusClass.ScopePlusLogin)
                                                 .AddScope(PlusClass.ScopePlusProfile)
                .Build();
            //gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
            //   .RequestEmail()
            //   .Build();
            //googleApiClient = new GoogleApiClient.Builder(Application.Context)
            //   .EnableAutoManage(activity, this)
            //   .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
            //   .Build();

        }

        public Task<SocialData> GetPersonData()
        {
            if(!googleApiClient.IsConnecting)
            {
                signInClicked = true;
                //ResolveSignInError();
            }
            _gPlusTask = new TaskCompletionSource<SocialData>();

            //Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            //Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivityForResult(signInIntent, googleSignInRequestCode);
            //Auth.GoogleSignInApi.SignOut(googleApiClient);
            googleApiClient.Connect();
            return _gPlusTask.Task;
        }

        private void ResolveSignInError()
        {
            if(googleApiClient.IsConnected)
            {
                // No need to resolve errors, already connected
                return;
            }
            if(connectionResult.HasResolution)
            {
                try
                {
                    intentInProgress = true;
                    Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartIntentSenderForResult(connectionResult.Resolution.IntentSender, 0, null, 0, 0, 0);
                }
                catch(Exception ex)
                {
                    intentInProgress = false;
                    googleApiClient.Connect();
                }
            }
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
                        _gPlusTask.TrySetResult(new SocialData()
                        {
                            Email = acct.Email,
                            FirstName = acct.GivenName,
                            LastName = acct.FamilyName,
                            Photo = acct.PhotoUrl?.ToString(),
                            Source = AuthorizationType.GPlus
                        });
                    }
                    else
                        _gPlusTask.TrySetResult(null);
                }
                else
                {
                    signInClicked = false;
                    _gPlusTask.TrySetResult(null);
                }
                intentInProgress = false;
                if(!googleApiClient.IsConnecting)
                    googleApiClient.Connect();
            }
        }

        public void ReleaseResources()
        {
            googleApiClient.Disconnect();
        }

        public void OnConnected(Bundle connectionHint)
        {
            // successfule log in
            signInClicked = false;

            if(PlusClass.PeopleApi.GetCurrentPerson(googleApiClient) != null)
            {
                Android.Gms.Plus.Model.People.IPerson plusUser = PlusClass.PeopleApi.GetCurrentPerson(googleApiClient);
                System.Diagnostics.Debug.WriteLine(plusUser.DisplayName);
            }

            System.Diagnostics.Debug.WriteLine("onConnected:" + connectionHint);
        }

        public void OnConnectionSuspended(int cause)
        {
            System.Diagnostics.Debug.WriteLine("OnConnectionSuspended:" + cause);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            if(!intentInProgress)
            {
                // store the connectionResult so that we can use it latter when the user clikcs 'sign-in'
                connectionResult = result;

                if(signInClicked)
                {
                    // the user already clicked 'sign-in' so we attemp to resolve all
                    // errors until the user is signed in, or the cancel
                    ResolveSignInError();
                }
            }
            Mvx.Trace("OnConnectionFailed:" + result);
        }
    }
}
