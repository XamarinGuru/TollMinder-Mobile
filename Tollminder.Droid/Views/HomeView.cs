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

namespace Tollminder.Droid.Views
{
	[Activity(Label = "Home", LaunchMode = LaunchMode.SingleTask, ScreenOrientation = ScreenOrientation.Portrait)]
	public class HomeView : MvxActivity<HomeViewModel>
    {
		int VOICE = 911;
		bool _speechRecognitionIsRunning;

		ITextToSpeechService TextToSpeech = Mvx.Resolve<ITextToSpeechService>();

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.homeView);

			this.AddLinqBinding(ViewModel, vm => vm.Question, (value) =>
			{
				if (!string.IsNullOrEmpty(value) && !_speechRecognitionIsRunning)
					StartSpeechRecognition(value);
			});
		}

		void StartSpeechRecognition(string question)
		{
			_speechRecognitionIsRunning = true;
			TextToSpeech.Speak(question);
			TextToSpeech.FinishedSpeaking += FinishedSpeaking;
		}

		void ProcessSpecchRecognition(IList<string> matches)
		{
			var answer = Mvx.Resolve<ITextFromSpeechMappingService>().DetectAnswer(matches);

			if (answer != Core.Models.AnswerType.Unknown)
			{
				TextToSpeech.Speak($"Your answer is {answer.ToString()}");
				_speechRecognitionIsRunning = false;
			}
			else
				StartSpeechRecognition(ViewModel.Question);
		}

		void FinishedSpeaking(object sender, string e)
		{
			if (e == ViewModel.Question)
			{
				var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
				voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
				voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, ViewModel.Question);
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

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == VOICE)
				ProcessSpecchRecognition(data?.GetStringArrayListExtra(RecognizerIntent.ExtraResults));

			base.OnActivityResult(requestCode, resultCode, data);
		}
    }
}