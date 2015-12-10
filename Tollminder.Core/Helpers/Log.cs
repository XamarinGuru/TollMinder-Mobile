using System;
using Cirrious.CrossCore;
using System.Reflection;

namespace Tollminder.Core.Helpers
{
	public static class Log
	{
		public static void LogMessage(string message)
		{			
			Mvx.Trace (Cirrious.CrossCore.Platform.MvxTraceLevel.Diagnostic, message, string.Empty);
		}
	}
}

