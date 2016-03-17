using System.Text;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;

namespace Tollminder.Core.Helpers
{
	public static class Log
	{
		public static StringBuilder _messageLog = new StringBuilder ();
		public static void LogMessage(string message)
		{
			_messageLog.Append (message);
			_messageLog.Append ("\n");
			Mvx.Resolve<IMvxMessenger> ().Publish (new LogUpdated (_messageLog));
			Mvx.Trace (MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, message, string.Empty);
		}
	}
}

