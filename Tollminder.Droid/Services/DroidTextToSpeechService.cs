using Android.App;
using Android.Speech.Tts;
using Tollminder.Core.Services;
using Android.Media;
using Android.Content;
using System;

namespace Tollminder.Droid.Services
{
	public class DroidTextToSpeechService : UtteranceProgressListener, ITextToSpeechService , TextToSpeech.IOnInitListener
    {
		public class TetxToSpeechEventArgs : EventArgs
		{
			public string Text { get; set; }
		}

		public event EventHandler<string> FinishedSpeaking;
		public bool IsEnabled { get; set; } = true;

		TextToSpeech _speaker;
		public TextToSpeech Speaker
		{
			get
			{
				return _speaker;
			}
		}

		public DroidTextToSpeechService ()
		{
			var context = Application.Context;
			_speaker = new TextToSpeech (context, this);
			_speaker.SetLanguage(new Java.Util.Locale("en-US"));
			_speaker.SetOnUtteranceProgressListener(this);
			AudioManager am = (AudioManager)context.GetSystemService(Context.AudioService);
			//am.SetStreamVolume(Stream.Music, am.GetStreamMaxVolume(Stream.Music), 0);		
			am.SetStreamVolume(Stream.Music, 5, 0);
		}

		#region ITextToSpeechService implementation

		public void Speak(string text)
        {      
			if (IsEnabled) {
				Speaker.Speak (text, QueueMode.Flush, null, text);
			}
        }

		#endregion

		#region IOnInitListener implementation

		public void OnInit (OperationResult status)
		{
		}

		public override void OnStart(string utteranceId)
		{
		}

		public override void OnDone(string utteranceId)
		{
			OnFinishedSpeaking(utteranceId);
		}

		public override void OnError(string utteranceId)
		{
			OnFinishedSpeaking(utteranceId);
		}

		void OnFinishedSpeaking(string text)
		{
			FinishedSpeaking?.Invoke(this, text);
		}

		#endregion
	
    }
}

