using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Speech;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.Views;

namespace Tollminder.Droid.Services
{
	public class DroidSpeechToTextService : ISpeechToTextService
	{
		public static int VoiceConstId = 911;
		TaskCompletionSource<bool> _recognitionTask;

		ITextFromSpeechMappingService _mappingService;
		ITextFromSpeechMappingService MappingService
		{
			get
			{
				return _mappingService ?? (_mappingService = Mvx.Resolve<ITextFromSpeechMappingService>());
			}
		}

		string _question;
		public string Question
		{
			get
			{
				return _question;
			}
			set
			{
				_question = value;
			}
		}

		public Task<bool> AskQuestion(string question)
		{
			_recognitionTask = new TaskCompletionSource<bool>();

			if (Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity == null)
			{
				var otherActivity = new Intent(Application.Context, typeof(HomeView));
				otherActivity.AddFlags(ActivityFlags.NewTask);
				Application.Context.StartActivity(otherActivity);

				EnsureActivityLoaded().Wait();
			}

			Mvx.Resolve<ITextToSpeechService>().Speak(question).Wait();

			Question = question;
			var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
			voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, question);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
			voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, "en-US");

			Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivityForResult(voiceIntent, VoiceConstId);

			return _recognitionTask.Task;
		}

		Task<bool> EnsureActivityLoaded()
		{
			var _ensureTask = new TaskCompletionSource<bool>();
			Mvx.Resolve<IMvxMessenger>().SubscribeOnThreadPoolThread<SpechRecognitionActivityLoadedMessage>(x =>
			{
				Console.WriteLine("Received SpechRecognitionActivityLoadedMessage");
				_ensureTask.SetResult(true);
			});
			return _ensureTask.Task;
		}

		void ISpeechToTextService.CheckResult(IList<string> matches)
		{
			var answer = MappingService.DetectAnswer(matches);

			if (answer != AnswerType.Unknown)
			{
				Mvx.Resolve<ITextToSpeechService>().Speak($"Your answer is {answer.ToString()}");
				_recognitionTask.TrySetResult(answer == AnswerType.Positive);
			}
			else
				AskQuestion(Question);
		}
	}
}

