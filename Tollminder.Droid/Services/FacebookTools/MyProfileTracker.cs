using System;
using Tollminder.Droid.Views;
using Xamarin.Facebook;

namespace Tollminder.Droid.Services.FacebookTools
{
    public class MyProfileTracker : ProfileTracker
    {
        readonly LoginView owner;

        public MyProfileTracker(LoginView owner)
        {
            this.owner = owner;
        }

        protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
        {
            owner.UpdateUI();
        }
    }
}
