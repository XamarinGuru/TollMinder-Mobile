using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IStoredSettingsBase
	{
		void Set<T>(T value, [CallerMemberNameAttribute] string key = "");
		T Get<T>(T defaultValue = default(T), [CallerMemberNameAttribute] string key = "");
	}
}

