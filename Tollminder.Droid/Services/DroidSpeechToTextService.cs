using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

		void ISpeechToTextService.CheckResult(IList<string> matches)
		{
			var answer = MappingService.DetectAnswer(matches);

			if (answer != AnswerType.Unknown)
			{
				Mvx.Resolve<ITextToSpeechService>().Speak($"Your answer is {answer.ToString()}");
				_recognitionTask.TrySetResult(true);
			}
			else
				AskQuestion(Question);
		}
	}
}

