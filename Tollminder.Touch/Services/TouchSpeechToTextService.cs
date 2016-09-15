using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Platform;
using OpenEars;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Touch.Views;

namespace Tollminder.Touch.Services
{
	public class TouchSpeechToTextService : ISpeechToTextService
	{
		OEEventsObserver observer;
		OEPocketsphinxController pocketSphinxController;
		OEFliteController fliteController;

		String pathToLanguageModel;
		String pathToDictionary;
		String pathToAcousticModel;
		String firstVoiceToUse;
		String secondVoiceToUse;

		bool _speechRecognitionIsRunning;

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

		public TouchSpeechToTextService()
		{
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

		class MyOpenEarsEventsObserverDelegate : OEEventsObserverDelegate
		{
			TouchSpeechToTextService _service;

			public MyOpenEarsEventsObserverDelegate(TouchSpeechToTextService service)
			{
				_service = service;
			}

			public override void PocketsphinxDidReceiveHypothesisrecognitionScoreutteranceID(string hypothesis, string recognitionScore, string utteranceID)
			{
				Console.WriteLine($"Heard: {hypothesis}, score {recognitionScore}, id {utteranceID}");
				_service.CheckResult(hypothesis.ToLower().Split(' '));
				_service.StopListening();
			}

			public override void AudioSessionInterruptionDidBegin()
			{
				Console.WriteLine("AudioSession interruption began.");
			}

			public override void AudioSessionInterruptionDidEnd()
			{
				Console.WriteLine("AudioSession interruption end.");
			}

			public override void AudioInputDidBecomeUnavailable()
			{
				Console.WriteLine("The audio input has become unavailable");
			}

			public override void AudioInputDidBecomeAvailable()
			{
				Console.WriteLine("The audio input is available.");
			}

			public override void AudioRouteDidChangeToRoute(string newRoute)
			{
				Console.WriteLine("Audio route change. The new route is " + newRoute);
			}

			public override void PocketsphinxDidStartCalibration()
			{
				Console.WriteLine("Pocketsphinx calibration has started.");
			}

			public override void PocketsphinxDidCompleteCalibration()
			{
				Console.WriteLine("Pocket calibration is complete");
			}

			public override void PocketsphinxRecognitionLoopDidStart()
			{
				Console.WriteLine("Pocketsphinx is starting up");
			}

			public override void PocketsphinxDidStartListening()
			{
				Console.WriteLine("Pocketsphinx is now listening");
			}

			public override void PocketsphinxDidDetectSpeech()
			{
				Console.WriteLine("Pocketsphinx has detected speech");
			}

			public override void PocketsphinxDidDetectFinishedSpeech()
			{
				Console.WriteLine("Pocketphinx has detected a second of silence, concluding utterance.");
			}

			public override void PocketsphinxDidStopListening()
			{
				Console.WriteLine("Pocketsphinx has stopped listening");
			}

			public override void PocketsphinxDidSuspendRecognition()
			{
				Console.WriteLine("Pocketsphinx has suspended recognition");
			}

			public override void PocketsphinxDidResumeRecognition()
			{
				Console.WriteLine("Pocketsphinx has resumed recognition");
			}

			public override void FliteDidStartSpeaking()
			{
				Console.WriteLine("Flite has started speaking");
			}

			public override void FliteDidFinishSpeaking()
			{
				Console.WriteLine("Flite has finished speaking.");
			}

			public override void PocketSphinxContinuousSetupDidFail()
			{

			}

		}

		public void AskQuestion(string question)
		{
			if (!_speechRecognitionIsRunning)
			{
				_speechRecognitionIsRunning = true;
				Mvx.Resolve<ITextToSpeechService>().Speak(question);
				Question = question;
				StartListening();
			}
		}

		public void CheckResult(IList<string> matches)
		{
			var answer = MappingService.DetectAnswer(matches);

			if (answer != AnswerType.Unknown)
			{
				Mvx.Resolve<ITextToSpeechService>().Speak($"Your answer is {answer.ToString()}");
			}
			else
				AskQuestion(Question);
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
			_speechRecognitionIsRunning = false;
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

