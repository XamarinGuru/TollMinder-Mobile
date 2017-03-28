using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Tollminder.Core.Services.SpeechRecognition;

namespace Tollminder.Droid.Services
{
    public class DroidPlatform : IPlatform
    {
        #region IPlatform implementation
        public bool IsAppInForeground
        {
            get
            {
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

                    if (appProcess.Importance == Importance.Foreground && appProcess.ProcessName.ToLowerInvariant() == packageName.ToLowerInvariant())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsMusicRunning
        {
            get
            {
                AudioManager manager = (AudioManager)Application.Context.GetSystemService(Context.AudioService);
                return manager.IsMusicActive;
            }
        }

        public void PauseMusic()
        {
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

        public void SetAudioEnabled(bool flag)
        {
            AudioManager amanager = (AudioManager)Application.Context.GetSystemService(Context.AudioService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Adjust value = (flag) ? Adjust.Unmute : Adjust.Mute;
                amanager.AdjustStreamVolume(Stream.Notification, value, 0);
                amanager.AdjustStreamVolume(Stream.Alarm, value, 0);
                amanager.AdjustStreamVolume(Stream.Music, value, 0);
                amanager.AdjustStreamVolume(Stream.Ring, value, 0);
                amanager.AdjustStreamVolume(Stream.System, value, 0);
            }
            else
            {
                amanager.SetStreamMute(Stream.Notification, !flag);
                amanager.SetStreamMute(Stream.Alarm, !flag);
                amanager.SetStreamMute(Stream.Music, !flag);
                amanager.SetStreamMute(Stream.Ring, !flag);
                amanager.SetStreamMute(Stream.System, !flag);
            }
        }

        #endregion

    }
}

