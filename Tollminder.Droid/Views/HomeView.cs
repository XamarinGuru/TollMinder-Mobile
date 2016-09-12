using Android.App;
using Android.Content;
using Android.OS;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;
using Tollminder.Core.ViewModels;
using Plugin.Permissions;
using Android.Content.PM;
using Android.Speech;
using Android.Widget;
using System.Linq;
using System.Collections.Generic;
using Tollminder.Core.Helpers;
using Tollminder.Droid.Services;

namespace Tollminder.Droid.Views
{
	[Activity(Label = "Home", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
	public class HomeView : MvxActivity<HomeViewModel>
    {
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.homeView);
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == DroidSpeechToTextService.VoiceConstId)
				Mvx.Resolve<ISpeechToTextService>().CheckResult(data?.GetStringArrayListExtra(RecognizerIntent.ExtraResults));

			base.OnActivityResult(requestCode, resultCode, data);
		}
    }
}