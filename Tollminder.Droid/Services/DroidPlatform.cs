using System;
using Tollminder.Core.Services;
using Android.Content;
using Android.App;
using System.Collections.Generic;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.Services
{
	public class DroidPlatform : IPlatform
	{
		#region IPlatform implementation
		public bool IsAppInForeground {
			get {
				Context context = Application.Context;
				ActivityManager activityManager = (ActivityManager)context.GetSystemService(Context.ActivityService);
				IList<Android.App.ActivityManager.RunningAppProcessInfo> appProcesses = activityManager.RunningAppProcesses;
				if (appProcesses == null)
				{
					return false;
				}
				string packageName = context.PackageName;
				foreach (Android.App.ActivityManager.RunningAppProcessInfo appProcess in appProcesses)
				{
					#if DEBUG
					Log.LogMessage(appProcess.ProcessName);
					#endif
					if (appProcess.Importance == Importance.Foreground && appProcess.ProcessName.ToLowerInvariant() == packageName.ToLowerInvariant())
					{
						return true;
					}
				}
				return false;
			}
		}
		#endregion
		
	}
}

