using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Preferences;
using Newtonsoft.Json;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.Views;

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
			if (string.IsNullOrEmpty(str))
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

        public List<StatesData> GetStatesFromJson()
        {
            List<StatesData> states = null;

            using (StreamReader read = new StreamReader(Application.Context.Assets.Open("states.json")))
            {
                string json = read.ReadToEnd();
                states = JsonConvert.DeserializeObject<List<StatesData>>(json);
            }
            return states;
        }
	}
}

