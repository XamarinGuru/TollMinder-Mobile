using System;
using Tollminder.Core.Models;

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

		public GeoLocation Location
		{
			get { return _storedSettingsBase.Get<GeoLocation>(); }
			set { _storedSettingsBase.Set<GeoLocation>(value); }
		}

		public TollGeolocationStatus Status
		{
			get { return _storedSettingsBase.Get<TollGeolocationStatus>(); }
			set { _storedSettingsBase.Set<TollGeolocationStatus>(value); }
		}
	}
}

