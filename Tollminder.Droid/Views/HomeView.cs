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

namespace Tollminder.Droid.Views
{
	[Activity(Label = "Home", LaunchMode = LaunchMode.SingleTask)]
	public class HomeView : MvxActivity<HomeViewModel>
    {
		int VOICE = 911;

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.homeView);

			var voiceBtn = FindViewById<Button>(Resource.Id.btnVoice);
			voiceBtn.Click += (sender, e) => StartSpeechRecognition();
        }

		void StartSpeechRecognition()
		{
			var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
			voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Are you on a entered a tollroad?");
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
			voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
			StartActivityForResult(voiceIntent, VOICE);
			Mvx.Resolve<INotifyService>().Notify("Are you on a entered a tollroad?");
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == VOICE)
			{
				var matches = data?.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
				if (matches != null && matches?.Count != 0)
				{
					string answer = matches.FirstOrDefault(x => x.Contains("no") || x.Contains("yes"));
					if (answer != null)
					{
						Mvx.Resolve<INotifyService>().Notify($"Your answer is {answer}");
					}
					else
						StartSpeechRecognition();
				}
			}

			base.OnActivityResult(requestCode, resultCode, data);
		}
    }
}