using System;
using Tollminder.Core.Services;
using Android.Content;
using Android.App;
using System.Collections.Generic;

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
					if (appProcess.Importance == Importance.Background && appProcess.ProcessName == packageName)
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

