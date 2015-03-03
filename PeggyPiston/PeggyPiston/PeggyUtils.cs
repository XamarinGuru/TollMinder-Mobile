using System;

namespace PeggyPiston
{
	public static class PeggyUtils
	{
		public static void DebugLog (String debugText, String channel=null) {
			if (channel == null) {
				channel = "Default";
			}
			System.Diagnostics.Debug.WriteLine (string.Format("--> [{0}] {1}", channel, debugText));
		}
	}
}

