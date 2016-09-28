using Android.App;
using Android.Speech.Tts;
using Tollminder.Core.Services;
using Android.Media;
using Android.Content;
using System;
using System.Threading.Tasks;
using MvvmCross.Platform;

namespace Tollminder.Droid.Services
{
	public class DroidTextToSpeechService : UtteranceProgressListener, ITextToSpeechService , TextToSpeech.IOnInitListener
    {
		public bool IsEnabled { get; set; } = true;

		IPlatform _platform;
		IPlatform Platform
		{
			get
			{
				return _platform ?? (_platform = Mvx.Resolve<IPlatform>());
			}
		}

		TextToSpeech _speaker;
		public TextToSpeech Speaker
		{
			get
			{
				return _speaker;
			}
		}

		bool DisableMusic { get; set; }

		public DroidTextToSpeechService ()
		{
			var context = Application.Context;
			_speaker = new TextToSpeech (context, this);
			_speaker.SetLanguage(new Java.Util.Locale("en-US"));
			_speaker.SetOnUtteranceProgressListener(this);
			AudioManager am = (AudioManager)context.GetSystemService(Context.AudioService);
#if RELEASE
			am.SetStreamVolume(Stream.Music, am.GetStreamMaxVolume(Stream.Music), 0);
#endif
#if DEBUG
			am.SetStreamVolume(Stream.Music, 5, 0);
#endif
		}

		#region ITextToSpeechService implementation
		TaskCompletionSource<bool> _speakTask;
		public Task Speak(string text, bool disableMusic = true)
        {
			DisableMusic = disableMusic && Platform.IsMusicRunning;
			if (DisableMusic)
				Platform.PauseMusic();
			_speakTask = new TaskCompletionSource<bool>();
			if (IsEnabled) {
				Speaker.Speak (text, QueueMode.Flush, null, text);
			}
			return _speakTask.Task;
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
			if (DisableMusic)
				Platform.PlayMusic();
			_speakTask.TrySetResult(true);
		}

		public override void OnError(string utteranceId)
		{
			Platform.PlayMusic();
			_speakTask.TrySetException(new Exception("Text to speech not working"));
		}

		#endregion
	
    }
}

