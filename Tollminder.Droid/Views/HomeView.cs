using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;
using Android.Content;
using Tollminder.Droid.Services;
using Cirrious.CrossCore;
using Tollminder.Core.Services;

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
    }
}