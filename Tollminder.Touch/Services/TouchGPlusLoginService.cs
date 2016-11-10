using System;
using System.Threading.Tasks;
using Foundation;
using Google.Core;
using Google.SignIn;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

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
                    Name = user?.Profile.Name,
                    Photo = user?.Profile.GetImageUrl(200)?.AbsoluteString,
                    Source = AuthorizationType.GPlus
                });
            }
            else
            {
                _gPlusTask?.TrySetResult(null);
            }
        }

        public Task<SocialData> GetPersonData()
        {
            _gPlusTask = new TaskCompletionSource<SocialData>();
            SignIn.SharedInstance.SignInUser();
            return _gPlusTask.Task;
        }

        public void Initialize()
        {
            SignIn.SharedInstance.DisconnectUser();
            SignIn.SharedInstance.Delegate = this;
        }

        public void ReleaseResources()
        {
            
        }
    }
}
