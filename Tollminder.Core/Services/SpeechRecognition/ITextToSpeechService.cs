using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services.SpeechRecognition
{
    public interface ITextToSpeechService
    {
        Task<bool> SpeakAsync(string text, bool disableMusic = false);
        bool IsEnabled { get; set; }
    }
}