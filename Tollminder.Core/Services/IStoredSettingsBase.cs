using System;
using System.Runtime.CompilerServices;

namespace Tollminder.Core.Services
{
	public interface IStoredSettingsBase
	{
		void Set<T>(T value, [CallerMemberNameAttribute] string key = "");
		T Get<T>(T defaultValue = default(T), [CallerMemberNameAttribute] string key = "");
	}
}

