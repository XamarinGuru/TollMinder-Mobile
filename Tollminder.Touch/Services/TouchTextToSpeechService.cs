using System;
using Tollminder.Core.Services;
using AVFoundation;

namespace Tollminder.Touch.Services
{
	public class TouchTextToSpeechService : ITextToSpeechService
    {
		private readonly AVSpeechSynthesizer _speechSynthesizer;

		public bool IsEnabled { get; set; }

        public TouchTextToSpeechService()
        {
			_speechSynthesizer = new AVSpeechSynthesizer ();			
        }

        #region ITextToSpeechService implementation

        public void Speak(string text)
        {
			if (IsEnabled) {
				var speechUtterance = new AVSpeechUtterance (text) {
					Rate = AVSpeechUtterance.MaximumSpeechRate / 2,
					Voice = AVSpeechSynthesisVoice.FromLanguage ("en-US"),
					Volume = 0.5f,
					PitchMultiplier = 1.0f
				};

	            _speechSynthesizer.SpeakUtterance (speechUtterance);
			}
        }

        #endregion
    }
}

