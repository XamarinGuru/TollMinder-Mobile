using System;
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

		public virtual void Notify (string message)
		{
			if (!_platform.IsAppInForeground) {
				_notificationSender.SendLocalNotification ("Toll Minder", message);
			}
			_textToSpeech.Speak (message);		
		}
	}
}