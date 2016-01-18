using Android.App;
using Android.Content;
using Android.OS;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;
using Tollminder.Core.ViewModels;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Home")]
	public class HomeView : MvxActivity<HomeViewModel>
    {
		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.homeView);

        }

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (requestCode == MotionActivityService.ResolutionRequest) {
				Mvx.Resolve<IMotionActivity> ().AuthInProgress = false;
				if (resultCode == Result.Ok) {
					Mvx.Resolve<IMotionActivity> ().StartDetection ();
				}

			}
		}
    }
}