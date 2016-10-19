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
        static int counter = 0;

		public static void LogMessage(string message)
		{
			//#if DEBUG
			Mvx.Trace (MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, message, string.Empty);

            if (counter > 500)
            {
                _messageLog.Clear();
                counter = 0;
            }

			_messageLog.AppendLine($"[{DateTime.Now}] {message}");
            counter++;
			try
			{
				Mvx.Resolve<IMvxMessenger>()?.Publish(new LogUpdated(new object()));
			}
			catch(Exception e)
			{
				Mvx.Trace(e.Message + e.StackTrace);
			}
			//#endif
		}
	}
}

