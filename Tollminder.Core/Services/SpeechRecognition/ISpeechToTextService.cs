using System.Threading.Tasks;

namespace Tollminder.Core.Services.SpeechRecognition
{
    public interface ISpeechToTextService
    {
        Task<bool> AskQuestionAsync(string question);
    }
}

