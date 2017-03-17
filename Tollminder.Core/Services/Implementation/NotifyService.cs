using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services.Implementation
{
	public class NotifyService : INotifyService
	{
		readonly IPlatform _platform;
		readonly ITextToSpeechService _textToSpeech;
		readonly INotificationSender _notificationSender;

		public NotifyService (IPlatform platform, ITextToSpeechService textToSpeech, INotificationSender notificationSender)
		{
			this._notificationSender = notificationSender;
			this._textToSpeech = textToSpeech;
			this._platform = platform;
		}

		public async virtual Task NotifyAsync (string message)
		{
			if (!_platform.IsAppInForeground) {
				_notificationSender.SendLocalNotification ("Toll Minder", message);
			}
			_textToSpeech.SpeakAsync (message).Wait();		
		}
	}
}