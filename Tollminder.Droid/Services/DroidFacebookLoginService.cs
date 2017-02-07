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
using Tollminder.Droid.Services.FacebookTools;
using Tollminder.Droid.Views;
using Xamarin.Auth;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;

namespace Tollminder.Droid.Services
{
    public class DroidFacebookLoginService : Java.Lang.Object, IFacebookLoginService, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback, IDroidSocialLogin
    {
        ICallbackManager callbackManager;
        //ICallbackManager facebookCallbackManager;

        TaskCompletionSource<SocialData> _facebookTask;

        public DroidFacebookLoginService()
        {
            //FacebookSdk.SdkInitialize(Application.Context);
        }
        protected Activity CurrentActivity
        {
            get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
        }
        public void Initialize()
        {
            //facebookCallbackManager = CallbackManagerFactory.Create();
            // create callback manager using CallbackManagerFactory
            //callbackManager = CallbackManagerFactory.Create();
            //LoginManager.Instance.RegisterCallback(callbackManager, this);
            //LoginManager.Instance.RegisterCallback(callbackManager, new MyFacebookCallback<LoginResult>((Tollminder.Droid.Views.LoginView)CurrentActivity));
        }

        public Task<SocialData> GetPersonData()
        {
            _facebookTask = new TaskCompletionSource<SocialData>();
            LoginToFacebook();
            //LoginManager.Instance.LogInWithReadPermissions(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, new List<string> { "public_profile", "email"});
            return _facebookTask.Task;
        }

        public void OnCancel()
        {
            Mvx.Trace("Facebook OnCancel");
            _facebookTask.TrySetResult(null);
        }

        public void OnError(FacebookException error)
        {
            Mvx.Trace("Facebook OnError" + error);
            _facebookTask.TrySetResult(null);
        }


        private void LoginToFacebook()
        {
            //isloggingin = true;
            var auth = new OAuth2Authenticator(
                clientId: "357633207963143",
                scope: "email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += LoginComplete;
            //Intent myIntent = new Intent(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, typeof(LoginView));
            Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivity(auth.GetUI(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity));
        }

        public async void LoginComplete(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (!e.IsAuthenticated)
            {
                System.Diagnostics.Debug.WriteLine("Not Authorised");
                return;
            }

            var accessToken = e.Account.Properties["access_token"].ToString();
            var expiresIn = Convert.ToDouble(e.Account.Properties["expires_in"]);
            var expiryDate = DateTime.Now + TimeSpan.FromSeconds(expiresIn);

            await GetAccountInformation(e.Account);

            // Now that we're logged in, make a OAuth2 request to get the user's id.
            //var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, e.Account);

            //request.GetResponseAsync().ContinueWith(t =>
            //{
            //    if (t.IsFaulted)
            //        System.Diagnostics.Debug.WriteLine("Error: " + t.Exception.InnerException.Message);
            //    else
            //    {
            //        string email;
            //        var emailRequest = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=email"), null, e.Account);
            //        emailRequest.GetResponseAsync().ContinueWith(response =>
            //        {
            //            Dictionary<string, string> emailResponseValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Result.GetResponseText());
            //            email = emailResponseValues.TryGetValue("email", out email);
            //            if (s)
            //            {
            //                System.Diagnostics.Debug.WriteLine(email);
            //                //model.UpdateProperties();
            //            }
            //            else
            //                System.Diagnostics.Debug.WriteLine("Error encountered at the absolutely last second");
            //        });
            //        string mail = t.Result.GetResponseText();

            //        Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(mail);
            //        bool s = values.TryGetValue("email", out email);
            //        if (s)
            //        {
            //            System.Diagnostics.Debug.WriteLine(email);
            //            //model.UpdateProperties();
            //        }
            //        else
            //            System.Diagnostics.Debug.WriteLine("Error encountered at the absolutely last second");
            //    }
            //});
        }

        private async Task GetAccountInformation(Account account, string valueYouWantToGet = null)
        {
            //var request = new OAuth2Request("GET", requestUri, null, account);
            await new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, account).GetResponseAsync().ContinueWith(async responseAsync =>
            {
                var responseName = await responseAsync;
                Dictionary<string, string> responseNameValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseName.GetResponseText());
                await new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=email"), null, account).GetResponseAsync().ContinueWith(async emailAsync =>
                {
                    var responseEmail = await emailAsync;
                    Dictionary<string, string> responseEmailValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseEmail.GetResponseText());

                    string name = GetValue("name", responseNameValues);

                    _facebookTask.TrySetResult(new SocialData()
                    {
                        Email = GetValue("email", responseEmailValues),
                        FirstName = name.Remove(0, name.IndexOf(' ')),
                        LastName = name.Substring(name.IndexOf(' ') + 1),
                        Source = AuthorizationType.Facebook
                    });
                });
            });
        }

        private string GetValue(string valueName, Dictionary<string, string> responseValues)
        {
            string value;
            bool isGetValue = responseValues.TryGetValue(valueName, out value);
            if (isGetValue)
                System.Diagnostics.Debug.WriteLine(value);
            else
                System.Diagnostics.Debug.WriteLine("Error encountered at the absolutely last second");
            return value;
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
                _facebookTask.TrySetResult(new SocialData()
                {
                    Email = acct.Email,
                    FirstName = acct.Name,
                    Photo = acct.PhotoUrl,
                    Source = AuthorizationType.Facebook
                });
            }
            else
                _facebookTask.TrySetResult(null);
        }

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            //facebookCallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            callbackManager.OnActivityResult(requestCode, Convert.ToInt32(resultCode), data);
        }

        public void ReleaseResources()
        {
        }
    }
}
