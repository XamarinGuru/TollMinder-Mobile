using Android.App;
using Android.Content.PM;
using Android.OS;
using Tollminder.Core.ViewModels;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "RegistrationView", Theme = "@style/AppTheme.NoActionBar", ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class RegistrationView : LifeCycleActivity<RegistrationViewModel>
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
