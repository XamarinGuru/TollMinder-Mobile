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

namespace Tollminder.Droid.Views
{
	[Activity(Label = "Home", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
	public class HomeView : MvxActivity<HomeViewModel>
    {
		int VOICE = 911;
		string text = "Are you entered the tollroad?";

		ITextToSpeechService TextToSpeech = Mvx.Resolve<ITextToSpeechService>();

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.homeView);

			var voiceBtn = FindViewById<Button>(Resource.Id.btnVoice);
			voiceBtn.Text = text;
			voiceBtn.Click += (sender, e) => StartSpeechRecognition();
        }

		void StartSpeechRecognition()
		{
			
			TextToSpeech.Speak(text);
			TextToSpeech.FinishedSpeaking += FinishedSpeaking;
		}

		void FinishedSpeaking(object sender, string e)
		{
			if (e == text)
			{
				var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
				voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
				voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, text);
				voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
				voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
				voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
				voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

				voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, "en-US");
				StartActivityForResult(voiceIntent, VOICE);
				TextToSpeech.FinishedSpeaking -= FinishedSpeaking;
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		string DetectAnswer(IList<string> matches)
		{
			if (matches == null)
				return null;

			if (matches.FirstOrDefault(x => x.Contains("yea") || x.Contains("yep") || x.Contains("yes")) != null)
				return "yes";	

			if (matches.FirstOrDefault(x => x.Contains("nope") || x.Contains("no")) != null)
				return "no";

			return null;
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == VOICE)
			{
				var answer = DetectAnswer(data?.GetStringArrayListExtra(RecognizerIntent.ExtraResults));

				if (answer != null)
					Mvx.Resolve<INotifyService>().Notify($"Your answer is {answer}");
				else
					StartSpeechRecognition();
			}

			base.OnActivityResult(requestCode, resultCode, data);
		}
    }
}