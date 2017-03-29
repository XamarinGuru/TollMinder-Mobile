using Android.App;
using Android.Content.PM;
using Android.OS;
using Tollminder.Core.ViewModels;
using Tollminder.Core.ViewModels.UserProfile;
using Tollminder.Droid.AndroidServices;
using MvvmCross.Droid.Views;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "RegistrationView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class RegistrationView : MvxActivity<RegistrationViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.registration_view);

            // hide keyboard when last editText lost focus
            FieldsService.LostFocusFromField(Resource.Id.phoneNumber_editText);
        }
    }
}
