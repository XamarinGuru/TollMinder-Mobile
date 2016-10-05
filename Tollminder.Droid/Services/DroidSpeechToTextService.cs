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
        bool _dialogManualAnswer;
		bool _isMusicRunning;
        Ringtone _ringtone;

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

		ITextToSpeechService _textToSpeechService;
		ITextToSpeechService TextToSpeechService
		{
			get
			{
				return _textToSpeechService ?? (_textToSpeechService = Mvx.Resolve<ITextToSpeechService>());
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
			_isMusicRunning = Platform.IsMusicRunning;

			if (_isMusicRunning)
				Platform.PauseMusic();
			_recognitionTask = new TaskCompletionSource<bool>();

			if (Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity == null)
			{
				var otherActivity = new Intent(Application.Context, typeof(HomeView));
				otherActivity.AddFlags(ActivityFlags.NewTask);
				Application.Context.StartActivity(otherActivity);

				EnsureActivityLoaded().Wait();
			}

			_handler = new Handler(Application.Context.MainLooper);

			TextToSpeechService.Speak(question, false).Wait();

			Question = question;

			StartSpeechRecognition();

			return _recognitionTask.Task;
		}

		void StartSpeechRecognition()
		{
            _handler.Post(() => _dialog?.Cancel());
            _dialogManualAnswer = false;
			_handler.Post(() =>
				{
					if (_dialog == null)
					{
						try
						{
                            _dialogManualAnswer = false;
							_dialog = new AlertDialog.Builder(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity)
									.SetTitle(Question)
									.SetMessage("Please, answer yes or no after the tone")
									.SetCancelable(false)
                                    .SetPositiveButton("Yes", (sender, e) => 
                        {
                            _dialogManualAnswer = true;
                            _speechRecognizer?.StopListening();
                            _recognitionTask.TrySetResult(true);
                        })
                                    .SetNegativeButton("No", (sender, e) =>
                        {
                            _dialogManualAnswer = true;
                            _speechRecognizer?.StopListening();
                            _recognitionTask.TrySetResult(false);
                        })
									.Show();
						}
						catch (Exception e)
						{
							Mvx.Trace(e.Message + e.StackTrace);
						}
					}
					else
					{
						_dialog.Show();
					}
				});


			TextToSpeechService.Speak("Please, answer yes or no after the tone", false).Wait();

			try
			{
                if (_ringtone == null)
				    _ringtone = RingtoneManager.GetRingtone(Application.Context, RingtoneManager.GetDefaultUri(RingtoneType.Notification));
				_ringtone.Play();
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

            if (!_dialogManualAnswer)
            {
                Task.Run(() =>
                {
                    _handler.Post(() => _dialog?.Cancel());
                    TextToSpeechService.Speak("Unknow answer, retry", false).Wait();
                    Task.Delay(1000).Wait();
                    StartSpeechRecognition();
            });
            }
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
            if (!_dialogManualAnswer)
            {
                Console.WriteLine("OnResults");

                var answer = MappingService.DetectAnswer(results.GetStringArrayList(SpeechRecognizer.ResultsRecognition));

                _handler.Post(() => _dialog?.Cancel());

                if (answer != AnswerType.Unknown)
                {
                    TextToSpeechService.Speak($"Your answer is {answer.ToString()}", false).Wait();
                    if (_isMusicRunning)
                        Platform.PlayMusic();
                    _recognitionTask.TrySetResult(answer == AnswerType.Positive);
                }
                else
                {
                    _handler.Post(() =>
                    {
                        _speechRecognizer.StopListening();
                    });
    				AskQuestion(Question);
			    }
            }
		}

		public void OnRmsChanged(float rmsdB)
		{
			Console.WriteLine("OnRmsChanged");
		}
	}
}

