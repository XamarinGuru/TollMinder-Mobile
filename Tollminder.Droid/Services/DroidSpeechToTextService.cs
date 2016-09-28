using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.Views;

namespace Tollminder.Droid.Services
{
	public class DroidSpeechToTextService : Java.Lang.Object, ISpeechToTextService, IRecognitionListener
	{
		//public static int VoiceConstId = 911;
		TaskCompletionSource<bool> _recognitionTask;

		SpeechRecognizer _speechRecognizer;
		Handler _handler;
		AlertDialog _dialog;

		IPlatform _platform;
		IPlatform Platform
		{
			get
			{
				return _platform ?? (_platform = Mvx.Resolve<IPlatform>());
			}
		}

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
			Platform.PauseMusic();
			_recognitionTask = new TaskCompletionSource<bool>();

			if (_handler == null)
				_handler = new Handler(Application.Context.MainLooper);

			if (Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity == null)
			{
				var otherActivity = new Intent(Application.Context, typeof(HomeView));
				otherActivity.AddFlags(ActivityFlags.NewTask);
				Application.Context.StartActivity(otherActivity);

				EnsureActivityLoaded().Wait();
			}

			Mvx.Resolve<ITextToSpeechService>().Speak(question, false).Wait();

			Question = question;

			StartSpeechRecognition();

			return _recognitionTask.Task;
		}

		void StartSpeechRecognition()
		{
			if (_dialog == null)
			{
				_handler.Post(() =>
				{
					try
					{ 
						_dialog = new AlertDialog.Builder(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity)
								.SetTitle(Question)
								.SetMessage("Please, answer yes or no after the tone")
						        .SetCancelable(false)
								.Show();
					}
					catch (Exception e)
					{
						Mvx.Trace(e.Message + e.StackTrace);
					}
				});
			}
			else
			{
				_dialog.Show();
			}

			Mvx.Resolve<ITextToSpeechService>().Speak("Please, answer yes or no after the tone", false).Wait();

			try
			{
				var notification = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
				Ringtone r = RingtoneManager.GetRingtone(Application.Context, notification);
				r.Play();
			}
			catch (Exception e)
			{
				Mvx.Trace(e.Message + e.StackTrace);
			}

			var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
			voiceIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, "com.tollminder");
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
			voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
			voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

			voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, "en-US");

			//Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.StartActivityForResult(voiceIntent, VoiceConstId);
			//Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.RunOnUiThread(() =>

			_handler.Post(() =>
		   {
				if (_speechRecognizer == null)
			   {
				   _speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(Application.Context);
				   _speechRecognizer.SetRecognitionListener(this);
			   }

				//Platform.MuteAudio();
			   _speechRecognizer.StartListening(voiceIntent);
		   });
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

		//void ISpeechToTextService.CheckResult(IList<string> matches)
		//{
		//	var answer = MappingService.DetectAnswer(matches);

		//	if (answer != AnswerType.Unknown)
		//	{
		//		Mvx.Resolve<ITextToSpeechService>().Speak($"Your answer is {answer.ToString()}");
		//		_recognitionTask.TrySetResult(answer == AnswerType.Positive);
		//	}
		//	else
		//		AskQuestion(Question);
		//}

		public void OnBeginningOfSpeech()
		{
			Console.WriteLine("OnBeginningOfSpeech");
		}

		public void OnBufferReceived(byte[] buffer)
		{
			Console.WriteLine("OnBufferReceived");
		}

		public void OnEndOfSpeech()
		{
			Console.WriteLine("OnEndOfSpeech");
		}

		public void OnError([GeneratedEnum] SpeechRecognizerError error)
		{
			Core.Helpers.Log.LogMessage("SpeechRecognizerError = " + error);

			if (error == SpeechRecognizerError.NoMatch || error == SpeechRecognizerError.SpeechTimeout)
				StartSpeechRecognition();
		}

		public void OnEvent(int eventType, Bundle @params)
		{
			Console.WriteLine("OnEvent");
		}

		public void OnPartialResults(Bundle partialResults)
		{
			Console.WriteLine("OnPartialResults");
		}

		public void OnReadyForSpeech(Bundle @params)
		{
			Console.WriteLine("OnReadyForSpeech");
		}

		public void OnResults(Bundle results)
		{
			Console.WriteLine("OnResults");

			var answer = MappingService.DetectAnswer(results.GetStringArrayList(SpeechRecognizer.ResultsRecognition));

			_handler.Post(() => _dialog?.Cancel());

			if (answer != AnswerType.Unknown)
			{
				Mvx.Resolve<ITextToSpeechService>().Speak($"Your answer is {answer.ToString()}", false);
				Platform.PlayMusic();
				_recognitionTask.TrySetResult(answer == AnswerType.Positive);
			}
			else
			{
				_speechRecognizer.StopListening();
				AskQuestion(Question);
			}
		}

		public void OnRmsChanged(float rmsdB)
		{
			Console.WriteLine("OnRmsChanged");
		}
	}
}

