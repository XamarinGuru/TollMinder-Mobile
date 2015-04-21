using System;
using Xamarin.Forms;

namespace PeggyPiston
{
	public static class PeggyUtils
	{

		private static DateTime JanFirst1970 = new DateTime(1970, 1, 1);
		public static long getTime() {
			return (long)((DateTime.Now.ToUniversalTime() - JanFirst1970).TotalMilliseconds + 0.5);
		}

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

