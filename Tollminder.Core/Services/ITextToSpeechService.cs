using System;

namespace Tollminder.Core.Services
{
    public interface ITextToSpeechService
    {
        void Speak(string text);
    }
}

