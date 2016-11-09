using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Facebook.LoginKit;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Touch.Services
{
    public class TouchFacebookLoginService : IFacebookLoginService
    {
        List<string> readPermissions = new List<string> { "public_profile", "email" };

        LoginButton loginView;

        public TouchFacebookLoginService()
        {
        }

        public void Initialize()
        {
            loginView = new LoginButton(new CGRect(51, 0, 218, 46))
            {
                LoginBehavior = LoginBehavior.Native,
                ReadPermissions = readPermissions.ToArray()
            };

            // Handle actions once the user is logged in
            loginView.Completed += (sender, e) =>
            {
                if (e.Error != null)
                {
                    // Handle if there was an error
                }

                if (e.Result.IsCancelled)
                {
                    // Handle if the user cancelled the login request
                }

                // Handle your successful login
            };
        }

        public Task<PersonData> Login()
        {
            
        }
    }
}
