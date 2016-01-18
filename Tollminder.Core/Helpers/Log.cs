using MvvmCross.Platform;

namespace Tollminder.Core.Helpers
{
	public static class Log
	{
		public static void LogMessage(string message)
		{			
			Mvx.Trace (MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, message, string.Empty);
		}
	}
}

