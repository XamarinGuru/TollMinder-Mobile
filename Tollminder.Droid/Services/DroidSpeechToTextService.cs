using System;
using System.Collections.Generic;
using Android.Content;
using Android.Speech;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
	public class DroidSpeechToTextService : ISpeechToTextService
	{
		ITextFromSpeechMappingService _mappingService;
		ITextFromSpeechMappingService MappingService
		{
			get
			{
				return _mappingService ?? (_mappingService = Mvx.Resolve<ITextFromSpeechMappingService>());
			}
		}

		public static int VoiceConstId = 911;

		bool _speechRecognitionIsRunning;

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

		public void AskQuestion(string question)
		{
			if (!_speechRecognitionIsRunning)
			{
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
				_speechRecognitionIsRunning = true;
			}
		}

		void ISpeechToTextService.CheckResult(IList<string> matches)
		{
			var answer = MappingService.DetectAnswer(matches);

			_speechRecognitionIsRunning = false;

			if (answer != AnswerType.Unknown)
			{
				Mvx.Resolve<ITextToSpeechService>().Speak($"Your answer is {answer.ToString()}");
			}
			else
				AskQuestion(Question);
		}
	}
}

