using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Newtonsoft.Json;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Notifications;
using Tollminder.Core.Services.SocialNetworks;
using Tollminder.Droid.Views;
using Xamarin.Auth;

namespace Tollminder.Droid.Services
{
    public class DroidFacebookLoginService : IFacebookLoginService
    {
        private Context context;
        bool isLogin = false;
        Intent loginViewIntent;
        TaskCompletionSource<SocialData> _facebookTask;

        public Task<SocialData> GetPersonDataAsync()
        {
            _facebookTask = new TaskCompletionSource<SocialData>();
            LoginToFacebook();
            return _facebookTask.Task;
        }

        private void LoginToFacebook()
        {
            var auth = new OAuth2Authenticator(
                clientId: context.Resources.GetString(Resource.String.app_id),
                scope: "email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            auth.Completed += LoginComplete;
            Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivity(auth.GetUI(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity));
        }

        public async void LoginComplete(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (!e.IsAuthenticated)
            {
                System.Diagnostics.Debug.WriteLine("Not Authorised");
                return;
            }
            isLogin = true;
            var accessToken = e.Account.Properties["access_token"];
            var expiresIn = Convert.ToDouble(e.Account.Properties["expires_in"]);
            var expiryDate = DateTime.Now + TimeSpan.FromSeconds(expiresIn);

            await GetAccountInformationAsync(e.Account);
            Mvx.Resolve<IProgressDialogManager>().ShowProgressDialog("Please wait!", "Facebook authorization. Data loading...");
        }

        private async Task GetAccountInformationAsync(Account account)
        {
            await new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, account).GetResponseAsync().ContinueWith(async responseAsync =>
            {
                context.StartActivity(loginViewIntent);
                var responseName = await responseAsync;

                Dictionary<string, string> responseNameValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseName.GetResponseText());
                await new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=email"), null, account).GetResponseAsync().ContinueWith(async emailAsync =>
                {
                    var responseEmail = await emailAsync;
                    Dictionary<string, string> responseEmailValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseEmail.GetResponseText());

                    string name = GetValue("name", responseNameValues);

                    _facebookTask.TrySetResult(new SocialData()
                    {
                        Id = GetValue("id", responseEmailValues),
                        Email = GetValue("email", responseEmailValues),
                        FullName = name,
                        FirstName = name.Substring(0, name.IndexOf(' ')),
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

        public void Initialize()
        {
            if (!isLogin)
            {
                context = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
                loginViewIntent = new Intent(context, typeof(LoginView));
            }
        }

        public void ReleaseResources()
        {
        }
    }
}
