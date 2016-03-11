using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace Tollminder.Droid
{
    [Activity(
        Label = "Tollminder.Droid"
        , MainLauncher = true
        , Icon = "@mipmap/icon"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }

		protected override void OnCreate (Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);
			Xamarin.Insights.Initialize ("2b455f0ac1fe12ddfc5b5ffae045c69e33a79c33", BaseContext);
		}

    }
}
