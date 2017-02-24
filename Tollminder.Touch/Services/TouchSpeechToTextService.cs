using System;
using System.Collections.Generic;
using System.IO;
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

namespace Tollminder.Touch.Services
{
	public class TouchSpeechToTextService : ISpeechToTextService
	{
        readonly ITextFromSpeechMappingService MappingService;
        readonly ITextToSpeechService TextToSpeechService;

		OEEventsObserver observer;
		OEPocketsphinxController pocketSphinxController;
		OEFliteController fliteController;
        AVAudioPlayer _audioPlayer;

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

            AskQuestionMethod(question);

			return _recognitionTask.Task;
		}

        void AskQuestionMethod(string question)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _error = new UIAlertView(question, "Please, answer after the signal", null, "NO", "Yes");
                _error.Clicked += (sender, buttonArgs) =>
                {
                    StopListening();
                    _recognitionTask.TrySetResult(buttonArgs.ButtonIndex != _error.CancelButtonIndex);
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
			var answer = MappingService.DetectAnswer(matches);

			_error.DismissWithClickedButtonIndex(0, true);
			                                                   
			if (answer != AnswerType.Unknown)
			{
				TextToSpeechService.Speak($"Your answer is {answer.ToString()}", false).ConfigureAwait(false);
				_recognitionTask.TrySetResult(answer == AnswerType.Positive);
			}
			else
                AskQuestionMethod(Question);
		}
           }
    // The following method is called by the timer's delegate.

    void CheckStatus(Object state)
    {
        TimerExampleState s = (TimerExampleState)state;
        s.counter++;
        Console.WriteLine("{0} Checking Status {1}.", DateTime.Now.TimeOfDay, s.counter);
        if (s.counter == 5)
        {
            // Shorten the period. Wait 10 seconds to restart the timer.
            (s.tmr).Change(10000, 100);
            Console.WriteLine("changed...");
        }
        if (s.counter == 10)
        {
            Console.WriteLine("disposing of timer...");
            s.tmr.Dispose();
            s.tmr = null;
        }
    }
		public void StartListening()
		{
            TimerExampleState s = new TimerExampleState();

        // Create the delegate that invokes methods for the timer.
        TimerCallback timerDelegate = new TimerCallback(CheckStatus);

        // Create a timer that waits one second, then invokes every second.
        Timer timer = new Timer(timerDelegate, s, 1000, 1000);

        // Keep a handle to the timer, so it can be disposed.
        s.tmr = timer;

        // The main thread does nothing until the timer is disposed.
        while (s.tmr != null)
            Thread.Sleep(0);
        Console.WriteLine("Timer example done.");

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

