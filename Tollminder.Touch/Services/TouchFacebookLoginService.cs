using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Foundation;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Touch.Services
{
    public class TouchFacebookLoginService : IFacebookLoginService
    {
        List<string> readPermissions = new List<string> { "public_profile", "email" };

        LoginManager manager;

        TaskCompletionSource<SocialData> _facebookTask;

        public TouchFacebookLoginService()
        {
        }

        public void Initialize()
        {
            Profile.EnableUpdatesOnAccessTokenChange(true);
            Settings.AppID = "194561500997971";
            Settings.DisplayName = "TollMinder";
            manager = new LoginManager();
            manager.LoginBehavior = LoginBehavior.Native;
        }

        public void ReleaseResources()
        {
            manager = null;
        }

        public Task<SocialData> GetPersonData()
        {
            _facebookTask = new TaskCompletionSource<SocialData>();

            manager.LogInWithReadPermissions(readPermissions.ToArray(), (res, e) => 
            {
                if (e != null)
                {
                    Mvx.Trace($"FacebookLoginButton error {e}");
                    _facebookTask.TrySetResult(null);
                    return;
                }

                if (AccessToken.CurrentAccessToken == null)
                {
                    Mvx.Trace($"Empty token");
                    _facebookTask.TrySetResult(null);
                    return;
                }

                GraphRequest request = new GraphRequest("/me", new NSDictionary("fields", "name,picture,email"), AccessToken.CurrentAccessToken.TokenString, null, "GET");
                request.Start(new GraphRequestHandler((connection, result, error) =>
                {
                    FacebookAccountResult acct = new FacebookAccountResult()
                    {
                        Id = result.ValueForKey(new NSString("id")).ToString(),
                        Name = result.ValueForKey(new NSString("name")).ToString(),
                        Email = result.ValueForKey(new NSString("email")).ToString()
                    };

                    if (acct != null)
                    {
                        Mvx.Trace($"Profile: {acct.Name}, {acct.Email}, {acct.PhotoUrl}");
                        _facebookTask.TrySetResult(new SocialData()
                        {
                            Email = acct.Name,
                            Name = acct.Name,
                            Photo = acct.PhotoUrl,
                            Source = AuthorizationType.Facebook
                        });
                    }
                }));
            });

            return _facebookTask.Task;
        }
    }
}
