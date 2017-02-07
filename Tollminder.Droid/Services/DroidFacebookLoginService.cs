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
    public class DroidFacebookLoginService : IFacebookLoginService
    {
        private Context context;
        Intent loginViewIntent;
        ProgressDialog progressDialog;
        TaskCompletionSource<SocialData> _facebookTask;

        public Task<SocialData> GetPersonData()
        {
            _facebookTask = new TaskCompletionSource<SocialData>();
            LoginToFacebook();
            return _facebookTask.Task;
        }

        private void LoginToFacebook()
        {
            var auth = new OAuth2Authenticator(
                clientId: "357633207963143",
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

            var accessToken = e.Account.Properties["access_token"].ToString();
            var expiresIn = Convert.ToDouble(e.Account.Properties["expires_in"]);
            var expiryDate = DateTime.Now + TimeSpan.FromSeconds(expiresIn);
            await GetAccountInformation(e.Account);
        }

        private async Task GetAccountInformation(Account account)
        {
            await new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, account).GetResponseAsync().ContinueWith(async responseAsync =>
            {
                context.StartActivity(loginViewIntent);
                progressDialog.Show();
                
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
                        FirstName = name.Remove(0, name.IndexOf(' ')),
                        LastName = name.Substring(name.IndexOf(' ') + 1),
                        Source = AuthorizationType.Facebook
                    });
                    progressDialog.Dismiss();
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

        private void ShowDialog()
        {
            progressDialog = new ProgressDialog(context);
            progressDialog.SetTitle("Facebook authorization");
            progressDialog.SetMessage("Please wait! Loading...");
            progressDialog.Show();
        }

        public void Initialize()
        {
            context = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
            loginViewIntent = new Intent(context, typeof(LoginView));
            progressDialog = new ProgressDialog(context);
            progressDialog.SetTitle("Facebook authorization");
            progressDialog.SetMessage("Please wait! Loading...");
        }

        public void ReleaseResources()
        {
        }
    }
}
