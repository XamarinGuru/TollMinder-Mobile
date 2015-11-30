using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using CoreLocation;
using UIKit;
using Tollminder.Core.Helpers;
using System.Linq;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : IGeoLocationWatcher
	{
		public GeoLocation Location { get; set; }

		private readonly CLLocationManager _locationManager;
		public TouchGeolocationWatcher ()
		{
			_locationManager = new CLLocationManager ();
			if (EnvironmentInfo.IsForIOSNine) {
				_locationManager.AllowsBackgroundLocationUpdates = true;				
			}
			_locationManager.RequestAlwaysAuthorization ();
		}

		#region IGeoLocationWatcher implementation

		public event EventHandler<LocationUpdatedEventArgs> LocationUpdatedEvent;


		public void StartGeolocationWatcher()
		{
			if (CLLocationManager.LocationServicesEnabled) {
				_locationManager.DesiredAccuracy = 1;
				_locationManager.LocationsUpdated += LocationUpdated;
			}
		}

		#endregion

		void LocationUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			var loc = e.Locations.Last ();
			var geoLocation = new GeoLocation()
			{
				Longitude = loc.Coordinate.Longitude,
				Latitude = loc.Coordinate.Latitude,
				Accuracy = loc.HorizontalAccuracy,
				Altitude = loc.Altitude,
				AltitudeAccuracy = loc.VerticalAccuracy
			};
			LocationUpdatedEvent (this, new LocationUpdatedEventArgs (geoLocation));
		}
	}
}

