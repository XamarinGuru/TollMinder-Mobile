using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Views;
using Tollminder.Core.ViewModels;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Profile", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class ProfileView : LifeCycleActivity<ProfileViewModel>
    {
        EditText lastEditText;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.profile_view);

            lastEditText = FindViewById<EditText>(Resource.Id.zip_code_editText);
            // hide keyboard when last editText lost focus
            lastEditText.FocusChange += (object sender, View.FocusChangeEventArgs e) => {
                if (!lastEditText.HasFocus)
                {
                    InputMethodManager inputManager = (InputMethodManager)GetSystemService(Context.InputMethodService);

                    inputManager.HideSoftInputFromWindow(lastEditText.WindowToken, 0);
                }
            };    
        }
    }
}
