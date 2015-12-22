using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Android.Content;
using Tollminder.Droid.Services;
using Cirrious.CrossCore;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Views
{
    [Activity(Label = "Home")]
    public class HomeView : MvxActivity
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