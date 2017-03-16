using Android.App;
using Android.Content.PM;
using Android.OS;
using Tollminder.Core.ViewModels;
using Tollminder.Core.ViewModels.UserProfile;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Profile", Theme = "@style/AppTheme", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class ProfileView : LifeCycleActivity<ProfileViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.profile_view);

            // hide keyboard when last editText lost focus
            FieldsService.LostFocusFromField(Resource.Id.zip_code_editText);  
        }
    }
}
