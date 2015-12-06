using System;
using Tollminder.Core.Services;
using Android.Speech.Tts;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Android.App;

namespace Tollminder.Droid.Services
{
	public class DroidTextToSpeechService : Java.Lang.Object, ITextToSpeechService , TextToSpeech.IOnInitListener
    {
        TextToSpeech speaker;
        string toSpeak;

        public DroidTextToSpeechService()
        {
        }

        #region ITextToSpeechService implementation

        public void Speak(string text)
        {
            toSpeak = text;
            if (speaker == null) {
                var context = Application.Context;
                speaker = new TextToSpeech (context, this);
            } else {
                var p = new Dictionary<string,string> ();
                speaker.Speak (toSpeak, QueueMode.Flush, p);
            }
        }

		#endregion

		#region IOnInitListener implementation

		public void OnInit (OperationResult status)
		{
			throw new NotImplementedException ();
		}

		#endregion
	
    }
}

