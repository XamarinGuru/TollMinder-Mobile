using Android.App;
using Android.Speech.Tts;
using Tollminder.Core.Services;
using Android.Media;
using Android.Content;

namespace Tollminder.Droid.Services
{
	public class DroidTextToSpeechService : Java.Lang.Object, ITextToSpeechService , TextToSpeech.IOnInitListener
    {
		public DroidTextToSpeechService ()
		{
			var context = Application.Context;
			_speaker = new TextToSpeech (context, this);
			AudioManager am = (AudioManager)context.GetSystemService(Context.AudioService);
			am.SetStreamVolume(Stream.Music, am.GetStreamMaxVolume(Stream.Music), 0);				
		}

		TextToSpeech _speaker;
		public TextToSpeech Speaker {
			get {				
				return _speaker; 
			}
		}	

        #region ITextToSpeechService implementation

		public bool IsEnabled { get; set; }

        public void Speak(string text)
        {      
			if (IsEnabled) {
				Speaker.Speak (text, QueueMode.Flush, null, null);
						
			}
        }

		#endregion

		#region IOnInitListener implementation

		public void OnInit (OperationResult status)
		{
		}

		#endregion
	
    }
}

