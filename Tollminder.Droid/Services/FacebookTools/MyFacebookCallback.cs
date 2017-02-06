using System;
using System.Diagnostics;
using Android.App;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Tollminder.Droid.Views;
using Xamarin.Facebook;

namespace Tollminder.Droid.Services.FacebookTools
{
    public class MyFacebookCallback<LoginResult> : Java.Lang.Object, IFacebookCallback where LoginResult : Java.Lang.Object
    {
        readonly LoginView owner;

        public MyFacebookCallback(LoginView owner)
        {
            this.owner = owner;
        }

        public void OnSuccess(Java.Lang.Object obj)
        {
            owner.UpdateUI();
        }

        public void OnCancel()
        {
            owner.UpdateUI();
        }

        public void OnError(FacebookException fbException)
        {
            Debug.WriteLine(fbException.Message);
            owner.UpdateUI();
        }

        private void ShowAlert()
        {
            IDialogInterfaceOnClickListener listener = null;
            new AlertDialog.Builder(owner)
                .SetTitle(Resource.String.cancelled)
                .SetMessage(Resource.String.permission_not_granted)
                .SetPositiveButton(Resource.String.ok, listener)
                .Show();
        }
    }
}
