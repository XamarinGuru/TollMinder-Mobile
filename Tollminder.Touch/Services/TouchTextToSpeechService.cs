using System;
using Tollminder.Core.Services;
using AVFoundation;
using System.Threading.Tasks;

namespace Tollminder.Touch.Services
{
	public class TouchTextToSpeechService : ITextToSpeechService
    {
		private readonly AVSpeechSynthesizer _speechSynthesizer;

		public bool IsEnabled { get; set; }

		TaskCompletionSource<bool> _speakTask;

        public TouchTextToSpeechService()
        {
			_speechSynthesizer = new AVSpeechSynthesizer ();			
        }

        #region ITextToSpeechService implementation

        public Task<bool> Speak(string text, bool disableMusic = false)
        {
			_speakTask = new TaskCompletionSource<bool>();
			if (IsEnabled) {
				var speechUtterance = new AVSpeechUtterance (text) {
					Rate = AVSpeechUtterance.MaximumSpeechRate / 2,
					Voice = AVSpeechSynthesisVoice.FromLanguage ("en-US"),
					Volume = 1.0f,
					PitchMultiplier = 1.0f
					//PreUtteranceDelay = 0.1
				};

				_speechSynthesizer.DidFinishSpeechUtterance += (sender, e) => 
				{
					_speakTask.TrySetResult(true);
				};

				if (_speechSynthesizer.Speaking)
				{
					_speechSynthesizer.StopSpeaking(AVSpeechBoundary.Immediate);
				}

	            _speechSynthesizer.SpeakUtterance (speechUtterance);
			}
			return _speakTask.Task;
        }

        #endregion
    }
}

