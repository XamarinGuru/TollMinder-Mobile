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
        Timer _timer;

        TaskCompletionSource<bool> _recognitionTask;

		UIAlertView _error;

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

            //AskQuestionMethod(question);

            _timer = new Core.Utils.Timer((s) => { AskQuestionMethod(question);}, question, new TimeSpan(0, 0, 0), new TimeSpan(0, 0, 15000), true);
			return _recognitionTask.Task;
		}

        void AskQuestionMethod(string question)
        {
            StopListening();
            //if (_recognitionTask.Task.IsCompleted)
            //{
            //    UIApplication.SharedApplication.InvokeOnMainThread(() =>
            //    {
            //        _error.DismissWithClickedButtonIndex(0, true);
            //    });
            //    _timer.Cancel();
            //    return;
            //}
            
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _error = new UIAlertView(question, "Please, answer after the signal", null, "NO", "Yes");
                _error.Clicked += (sender, _buttonArgs) =>
                {
                    StopListening();
                    _recognitionTask.TrySetResult(_buttonArgs.ButtonIndex != _error.CancelButtonIndex);
                };
                _error.Show();
            });

            TextToSpeechService.Speak(question, false).Wait();

            TextToSpeechService.Speak("Please, answer after the signal", false).Wait();

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

		public void CheckResult(IList<string> matches)
		{
			_answer = MappingService.DetectAnswer(matches);

			_error.DismissWithClickedButtonIndex(0, true);
			                                                   
			if (_answer != AnswerType.Unknown)
			{
				TextToSpeechService.Speak($"Your answer is {_answer.ToString()}", false).ConfigureAwait(false);
				_recognitionTask.TrySetResult(_answer == AnswerType.Positive);
			}
			else
                AskQuestionMethod(Question);
		}

        // The following method is called by the timer's delegate.
        class TimerExampleState
        {
            public int counter = 0;
            public System.Threading.Timer tmr;
        }
        void CheckStatus(Object state)
        {
            TimerExampleState s = (TimerExampleState)state;
            s.counter++;
            Debug.WriteLine("{0} Checking Status {1}.", DateTime.Now.TimeOfDay, s.counter);
            if (s.counter == 5)
            {
                // Shorten the period. Wait 10 seconds to restart the timer.
                (s.tmr).Change(10000, 100);
                Debug.WriteLine("changed...");
            }
            if (s.counter == 10)
            {
                Debug.WriteLine("disposing of timer...");
                s.tmr.Dispose();
                s.tmr = null;
            }
        }
		public void StartListening()
		{
            //TimerExampleState s = new TimerExampleState();

            //// Create the delegate that invokes methods for the timer.
            //TimerCallback timerDelegate = new TimerCallback(CheckStatus);

            //    // Create a timer that waits one second, then invokes every second.
            //    System.Threading.Timer timer = new System.Threading.Timer(timerDelegate, s, 1000, 1000);

            //// Keep a handle to the timer, so it can be disposed.
                                        //s.tmr = timer;

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
                _error.DismissWithClickedButtonIndex(0, true);
            });
            if (_recognitionTask.Task.IsCompleted)
            {
                _timer.Cancel();
                //return;
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

