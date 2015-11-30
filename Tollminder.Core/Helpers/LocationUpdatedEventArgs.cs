using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Helpers
{
	public class LocationUpdatedEventArgs : EventArgs
	{
		private GeoLocation _geolocation;
		public LocationUpdatedEventArgs (GeoLocation location)
		{
			_geolocation = location;
		}

		public GeoLocation Location
		{
			get { return _geolocation; }
		}
	}
}

