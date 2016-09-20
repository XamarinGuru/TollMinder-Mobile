using System;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Preferences;
using Newtonsoft.Json;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
	public class DroidStoredSettingsBase : IStoredSettingsBase
	{
		ISharedPreferences prefs;

		public DroidStoredSettingsBase()
		{
			prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
		}

		public T Get<T>(T defaultValue = default(T), [CallerMemberNameAttribute] string key = "")
		{
			var str = prefs.GetString(key, null);
			if (str == null || string.IsNullOrEmpty(str))
			{
				return defaultValue;
			}
			var obj = JsonConvert.DeserializeObject<T>(str);
			return obj;
		}

		public void Set<T>(T value, [CallerMemberNameAttribute] string key = "")
		{
			var str = JsonConvert.SerializeObject(value);
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString(key, str);
			editor.Apply();
		}
	}
}

