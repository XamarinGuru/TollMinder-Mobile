using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Media;
using Tollminder.Core.Helpers;
using Tollminder.Core.Services;

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
					Log.LogMessage(appProcess.ProcessName);

					if (appProcess.Importance == Importance.Foreground && appProcess.ProcessName.ToLowerInvariant() == packageName.ToLowerInvariant())
					{
						return true;
					}
				}
				return false;
			}
		}

		public void PauseMusic()
		{
			//AudioManager amanager = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
			//amanager.SetStreamVolume(Stream.Music, 2, VolumeNotificationFlags.ShowUi);
			//amanager.SetStreamMute(Stream.Music, true);

			Intent i = new Intent("com.android.music.musicservicecommand");
			i.PutExtra("command", "pause");

			Application.Context.SendBroadcast(i);
		}

		public void PlayMusic()
		{
			//AudioManager amanager = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
			//amanager.SetStreamMute(Stream.Music, false);
			//amanager.SetStreamVolume(Stream.Music, 20, VolumeNotificationFlags.ShowUi);

			Intent i = new Intent("com.android.music.musicservicecommand");
			i.PutExtra("command", "play");

			Application.Context.SendBroadcast(i);
		}
		#endregion
		
	}
}

