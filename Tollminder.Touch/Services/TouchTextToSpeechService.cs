using System;
using Tollminder.Core.Services;
using AVFoundation;

namespace Tollminder.Touch.Services
{
    public class TouchTextToSpeechService : ITextToSpeechService
    {
        public TouchTextToSpeechService()
        {
        }

        #region ITextToSpeechService implementation

        public void Speak(string text)
        {
            var speechSynthesizer = new AVSpeechSynthesizer ();

            var speechUtterance = new AVSpeechUtterance (text) {
                Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
                Voice = AVSpeechSynthesisVoice.FromLanguage ("en-US"),
                Volume = 0.5f,
                PitchMultiplier = 1.0f
            };

            speechSynthesizer.SpeakUtterance (speechUtterance);

        }

        #endregion
    }
}

