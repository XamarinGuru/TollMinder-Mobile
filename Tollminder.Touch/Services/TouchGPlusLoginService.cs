using System;
using System.Threading.Tasks;
using Foundation;
using Google.Core;
using Google.SignIn;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services.SocialNetworks;

namespace Tollminder.Touch.Services
{
    public class TouchGPlusLoginService : NSObject, ISignInDelegate, IGPlusLoginService
    {
        TaskCompletionSource<SocialData> _gPlusTask;

        public TouchGPlusLoginService()
        {
        }

        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            if (user != null)
            {
                Mvx.Trace($"Profile: {user?.Profile.Name}, {user?.Profile.Email}, {user?.Profile.GetImageUrl(200)}");
                _gPlusTask.TrySetResult(new SocialData()
                {
                    Email = user?.Profile.Email,
                    FirstName = user?.Profile.GivenName,
                    LastName = user?.Profile.FamilyName,
                    Photo = user?.Profile.GetImageUrl(200)?.AbsoluteString,
                    Source = AuthorizationType.GPlus
                });
            }
            else
            {
                _gPlusTask?.TrySetResult(null);
            }
        }

        public Task<SocialData> GetPersonDataAsync()
        {
            _gPlusTask = new TaskCompletionSource<SocialData>();
            SignIn.SharedInstance.SignInUser();
            return _gPlusTask.Task;
        }

        public void Initialize()
        {
            string clientId = "41587140639-m2eom7qq6394aoa96nrrh30hd4p28o57.apps.googleusercontent.com"; //"382677639037-husbvvt31q4ik2mbfrinmsdu2stljhot.apps.googleusercontent.com";//

            NSError configureError;
            Context.SharedInstance.Configure(out configureError);
            if (configureError != null)
            {
                // If something went wrong, assign the clientID manually
                Console.WriteLine("Error configuring the Google context: {0}", configureError);
                SignIn.SharedInstance.ClientID = clientId;
            }

            SignIn.SharedInstance.DisconnectUser();
            SignIn.SharedInstance.Delegate = this;
        }

        public void ReleaseResources()
        {

        }
    }
}
