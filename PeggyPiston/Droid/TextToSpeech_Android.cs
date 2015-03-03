using System;
using Android.Speech.Tts;
using Xamarin.Forms;
using PeggyPiston.Droid;
using System.Collections.Generic;

[assembly: Dependency (typeof (TextToSpeech_Android))]

namespace PeggyPiston.Droid
{
	public class TextToSpeech_Android : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
	{
		protected readonly string logChannel = "TextToSpeech_Android";

		TextToSpeech speaker; string toSpeak;
		public TextToSpeech_Android () {}

		public void Speak (string text)
		{
			var c = Forms.Context; 
			toSpeak = text;
			if (speaker == null) {
				speaker = new TextToSpeech (c, this);
			} else {
				var p = new Dictionary<string,string> ();
				speaker.Speak (toSpeak, QueueMode.Flush, p);
				PeggyUtils.DebugLog("spoke " + toSpeak, logChannel);
			}
		}

		#region IOnInitListener implementation
		public void OnInit (OperationResult status)
		{
			if (status.Equals (OperationResult.Success)) {
				PeggyUtils.DebugLog("speaker init", logChannel);
				var p = new Dictionary<string,string> ();
				speaker.Speak (toSpeak, QueueMode.Flush, p);
			} else {
				PeggyUtils.DebugLog("was quiet", logChannel);
			}
		}
		#endregion
	}
}

