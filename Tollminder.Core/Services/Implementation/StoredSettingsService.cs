using System;
namespace Tollminder.Core.Services.Implementation
{
	public class StoredSettingsService : IStoredSettingsService
	{
		readonly IStoredSettingsBase _storedSettingsBase;

		public StoredSettingsService(IStoredSettingsBase storedSettingsBase)
		{
			_storedSettingsBase = storedSettingsBase;
		}

		public bool GeoWatcherIsRunning
		{
			get { return _storedSettingsBase.Get<bool>(); }
			set { _storedSettingsBase.Set<bool>(value); }
		}
	}
}

