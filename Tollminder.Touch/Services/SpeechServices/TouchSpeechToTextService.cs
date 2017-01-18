using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using AudioToolbox;
using AVFoundation;
using Foundation;
using MvvmCross.Platform;
using OpenEars;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Touch.Views;
using UIKit;

namespace Tollminder.Touch.Services.SpeechServices
{
	public class TouchSpeechToTextService : ISpeechToTextService
	{
        readonly ITextFromSpeechMappingService MappingService;
        readonly ITextToSpeechService TextToSpeechService;

		OEEventsObserver observer;
		OEPocketsphinxController pocketSphinxController;
		OEFliteController fliteController;
        AVAudioPlayer _audioPlayer;
        AnswerType _answer;
        Core.Utils.Timer _timer;
        bool isTimeStarted;

        TaskCompletionSource<bool> _recognitionTask;

		UIAlertView _questionAlert;

		string pathToLanguageModel;
		string pathToDictionary;
		string pathToAcousticModel;
		string firstVoiceToUse;
		string secondVoiceToUse;

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

		public TouchSpeechToTextService()
		{
            MappingService = Mvx.Resolve<ITextFromSpeechMappingService>();
            TextToSpeechService = Mvx.Resolve<ITextToSpeechService>();

			observer = new OEEventsObserver();
			observer.WeakDelegate = new MyOpenEarsEventsObserverDelegate(this);
			pocketSphinxController = new OEPocketsphinxController();
			fliteController = new OEFliteController();

			firstVoiceToUse = "cmu_us_slt";
			secondVoiceToUse = "cmu_us_rms";
			pathToLanguageModel = NSBundle.MainBundle.ResourcePath + System.IO.Path.DirectorySeparatorChar + "OpenEars1.languagemodel";
			pathToDictionary = NSBundle.MainBundle.ResourcePath + System.IO.Path.DirectorySeparatorChar + "OpenEars1.dic";
			pathToAcousticModel = NSBundle.MainBundle.ResourcePath + System.IO.Path.DirectorySeparatorChar + "AcousticModelEnglish.bundle";
		}
		
		public Task<bool> AskQuestion(string question)
		{
			_recognitionTask = new TaskCompletionSource<bool>();

            TimerManager(question);

			return _recognitionTask.Task;
		}

        async Task AskQuestionMethod(string question)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _questionAlert = new UIAlertView(question, "Please, answer after the signal", null, "NO", "Yes");
                _questionAlert.Clicked += (sender, _buttonArgs) =>
                {
                    TimerManager(question);
                    _recognitionTask.TrySetResult(_buttonArgs.ButtonIndex != _questionAlert.CancelButtonIndex);
                };
                _questionAlert.Show();
            });
            if (await TextToSpeechService.Speak(question, false))
            {
                if (await TextToSpeechService.Speak("Please, answer after the signal", false))
                {

                    UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Playback);
                        AVAudioSession.SharedInstance().SetActive(true);
                    //SystemSound notificationSound = SystemSound.FromFile(@"/System/Library/Audio/UISounds/jbl_begin.caf");
                    //notificationSound.AddSystemSoundCompletion(SystemSound.Vibrate.PlaySystemSound);
                    //notificationSound.PlaySystemSound();

                    _audioPlayer = AVAudioPlayer.FromUrl(NSUrl.FromFilename(Path.Combine("Sounds", "tap.aif")));
                        _audioPlayer.PrepareToPlay();
                        _audioPlayer.Play();
                    });

                    Question = question;
                    StartListening();
                }
            }
        }

        public void CheckResult(IList<string> matches)
        {
            TimerManager(Question);
            _answer = MappingService.DetectAnswer(matches);

            if (_answer != AnswerType.Unknown)
            {
                TextToSpeechService.Speak($"Your answer is {_answer.ToString()}", false).ConfigureAwait(false);
                _recognitionTask.TrySetResult(_answer == AnswerType.Positive);
            }
            else
                TimerManager(Question);
        }

		public void StartListening()
		{
            pocketSphinxController.StartListeningWithLanguageModelAtPathdictionaryAtPathlanguageModelIsJSGF(
				pathToLanguageModel,
				pathToDictionary,
				pathToAcousticModel,
				false
			);
		}

		public void StopListening()
		{
			pocketSphinxController.StopListening();
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _questionAlert?.DismissWithClickedButtonIndex(0, true);
            });
		}

        private void TimerManager(string question)
        {
            StopListening();
            if (!isTimeStarted)
            {
                isTimeStarted = true;
                _timer = new Core.Utils.Timer((s) => { AskQuestionMethod(question); }, question, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 15), true);
            }
            else
            {
                _timer.Cancel();
                isTimeStarted = false;
            }
        }

		public void SuspendRecognition()
		{
			pocketSphinxController.SuspendRecognition();
		}

		public void ResumeRecognition()
		{
			pocketSphinxController.ResumeRecognition();
		}
	}
}

