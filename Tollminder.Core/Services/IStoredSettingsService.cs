using System;
namespace Tollminder.Core.Services
{
	public interface IStoredSettingsService
	{
		bool GeoWatcherIsRunning { get; set; }
	}
}

