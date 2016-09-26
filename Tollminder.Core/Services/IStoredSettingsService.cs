using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IStoredSettingsService
	{
		bool GeoWatcherIsRunning { get; set; }
		GeoLocation Location { get; set; }
		TollGeolocationStatus Status { get; set; }
	}
}

