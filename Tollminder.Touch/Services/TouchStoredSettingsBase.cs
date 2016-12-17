using System;
using System.Runtime.CompilerServices;
using Foundation;
using Newtonsoft.Json;
using Tollminder.Core.Services;

namespace Tollminder.Touch.Services
{
	public class TouchStoredSettingsBase: IStoredSettingsBase
	{
		private const string NULL = "!NULL";
		private readonly NSUserDefaults _preferences;

		public TouchStoredSettingsBase()
		{
			_preferences = NSUserDefaults.StandardUserDefaults;
		}

		#region ISettingsBase implementation

		public void Set<T>(T value, [CallerMemberNameAttribute] string key = "")
		{
			var str = JsonConvert.SerializeObject(value);
			_preferences.SetString(str ?? NULL, key);
		}

		public T Get<T>(T defaultValue = default(T), [CallerMemberNameAttribute] string key = "")
		{
 			var str = _preferences.StringForKey(key);
			if (str == NULL || string.IsNullOrEmpty(str))
			{
				return defaultValue;
			}
			var obj = JsonConvert.DeserializeObject<T>(str);
			return obj;
		}

		#endregion

	}
}

