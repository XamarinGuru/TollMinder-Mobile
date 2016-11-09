using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Java.Lang;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Newtonsoft.Json;
using Org.Json;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.Inerfaces;
using Tollminder.Droid.Models;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace Tollminder.Droid.Services
{
    public class DroidFacebookLoginService : Java.Lang.Object, IFacebookLoginService, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback, IDroidSocialLogin
    {
        ICallbackManager facebookCallbackManager;

        TaskCompletionSource<PersonData> _facebookTask;

        public DroidFacebookLoginService()
        {
            FacebookSdk.SdkInitialize(Application.Context);
        }

        public void Initialize()
        {
            facebookCallbackManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(facebookCallbackManager, this);
        }

        public Task<PersonData> GetPersonData()
        {
            _facebookTask = new TaskCompletionSource<PersonData>();
            LoginManager.Instance.LogInWithReadPermissions(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, new List<string> { "public_profile", "email"});
            return _facebookTask.Task;
        }

        public void OnCancel()
        {
            Mvx.Trace("Facebook OnCancel");
        }

        public void OnError(FacebookException error)
        {
            Mvx.Trace("Facebook OnError" + error);
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            GraphRequest request = GraphRequest.NewMeRequest(AccessToken.CurrentAccessToken, this);

            Bundle parameters = new Bundle();
            parameters.PutString("fields", "id,name,email");
            request.Parameters = parameters;
            request.ExecuteAsync();
            LoginManager.Instance.LogOut();
        }

        public void OnCompleted(JSONObject json, GraphResponse response)
        {
            FacebookAccountResult acct = JsonConvert.DeserializeObject<FacebookAccountResult>(json.ToString());

            if (acct != null)
            {
                Mvx.Trace($"Profile: {acct.Name}, {acct.Email}, {acct.PhotoUrl}");
                _facebookTask.TrySetResult(new PersonData()
                {
                    Email = acct.Email,
                    Name = acct.Name,
                    Photo = acct.PhotoUrl,
                    Source = AuthorizationType.Facebook
                });
            }
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            facebookCallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        public void ReleaseResources()
        {
        }
    }
}
