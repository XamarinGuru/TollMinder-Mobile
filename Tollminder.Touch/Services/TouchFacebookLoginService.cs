using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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
            Facebook.CoreKit.Profile.EnableUpdatesOnAccessTokenChange(true);
            Settings.AppID = "357633207963143";//"194561500997971";
            Settings.DisplayName = "Tollminder";
            manager = new LoginManager();
            manager.LoginBehavior = LoginBehavior.Browser;
            Facebook.CoreKit.Profile.Notifications.ObserveDidChange((sender, e) =>
            {
                if (e.NewProfile == null)
                    return;

                Debug.WriteLine(e.NewProfile.Name);
            });
        }

        public void ReleaseResources()
        {
            manager = null;
        }

        public Task<SocialData> GetPersonData()
        {
            _facebookTask = new TaskCompletionSource<SocialData>();
            CancellationToken canellationToken = new CancellationToken(true);

            manager.LogInWithReadPermissions(readPermissions.ToArray(), (res, e) => 
            {
                if (e != null)
                {
                    Mvx.Trace($"FacebookLoginButton error {e}");
                    _facebookTask.TrySetResult(null);
                    return;
                }

                try
                {
                    if (AccessToken.CurrentAccessToken == null)
                    {
                        Mvx.Trace($"Empty token");
                        manager.LogOut();
                        _facebookTask.TrySetCanceled(canellationToken);
                        //GetPersonData();
                        return;
                    }
                }
                catch(Exception ex)
                { 
                    Debug.WriteLine(ex.Message, ex.StackTrace);
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
                            Email = acct.Email,
                            FirstName = acct.Name,
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
