using System;
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
			//#if DEBUG
			Mvx.Trace (MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, message, string.Empty);

			_messageLog.AppendLine($"[{DateTime.Now}] {message}");
			Mvx.Resolve<IMvxMessenger>().Publish(new LogUpdated(new object()));
			//#endif
		}
	}
}

