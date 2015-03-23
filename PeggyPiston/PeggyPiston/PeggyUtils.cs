using System;
using Xamarin.Forms;

namespace PeggyPiston
{
	public static class PeggyUtils
	{
		public static void DebugLog (String debugText, String channel=null) {
			if (channel == null) {
				channel = "Default";
			}

			if (channel == PeggyConstants.channelVoice) {
				DependencyService.Get<ITextToSpeech> ().Speak (debugText);

			} else {
				System.Diagnostics.Debug.WriteLine (string.Format ("--> [{0}] {1}", channel, debugText));
			}
		}
	}
}

