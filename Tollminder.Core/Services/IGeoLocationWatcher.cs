using System;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Services
{
	public interface IGeoLocationWatcher
	{
		event EventHandler<LocationUpdatedEventArgs> LocationUpdatedEvent;
		GeoLocation Location { get; set; }	
		void StopGeolocationWatcher ();
		void StartGeolocationWatcher();
	}
}