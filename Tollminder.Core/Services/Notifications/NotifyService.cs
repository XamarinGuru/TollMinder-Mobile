using System;
using System.Threading.Tasks;
using Tollminder.Core.Services.SpeechRecognition;

namespace Tollminder.Core.Services.Notifications
{
    public class NotifyService : INotifyService
    {
        readonly IPlatform _platform;
        readonly ITextToSpeechService _textToSpeech;
        readonly INotificationSender _notificationSender;

        public NotifyService(IPlatform platform, ITextToSpeechService textToSpeech, INotificationSender notificationSender)
        {
            this._notificationSender = notificationSender;
            this._textToSpeech = textToSpeech;
            this._platform = platform;
        }

        public async virtual Task NotifyAsync(string message)
        {
            if (!_platform.IsAppInForeground)
            {
                _notificationSender.SendLocalNotification("Toll Minder", message);
            }
            await _textToSpeech.SpeakAsync(message);
        }
    }
}