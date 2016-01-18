using Android.App;
using Android.Speech.Tts;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
	public class DroidTextToSpeechService : Java.Lang.Object, ITextToSpeechService , TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;          

        #region ITextToSpeechService implementation

		public bool IsEnabled { get; set; }

        public void Speak(string text)
        {      
			if (IsEnabled) {
				if (speaker == null) {
					var context = Application.Context;
					speaker = new TextToSpeech (context, this);
				} else {
					speaker.Speak (text, QueueMode.Flush, null, null);
				}				
			}
        }

		#endregion

		#region IOnInitListener implementation

		public void OnInit (OperationResult status)
		{
			speaker.SetLanguage (Java.Util.Locale.English);
			speaker.SetVoice (speaker.DefaultVoice);
		}

		#endregion
	
    }
}

