using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using CoreLocation;
using UIKit;
using Tollminder.Core.Helpers;
using System.Linq;
using Cirrious.CrossCore;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
using MessengerHub;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : IGeoLocationWatcher
	{
		public GeoLocation Location { get; set; }

		private readonly CLLocationManager _locationManager;
		public TouchGeolocationWatcher ()
		{
			_locationManager = new CLLocationManager ();
			_locationManager.RequestAlwaysAuthorization ();

			if (EnvironmentInfo.IsForIOSNine) {
				_locationManager.AllowsBackgroundLocationUpdates = true;				
			}
		}

		#region IGeoLocationWatcher implementation

		public event EventHandler<LocationUpdatedEventArgs> LocationUpdatedEvent;


		public void StartGeolocationWatcher()
		{
			if (CLLocationManager.LocationServicesEnabled) {
				_locationManager.DesiredAccuracy = 1;
				_locationManager.LocationsUpdated += LocationUpdated;
				_locationManager.StartUpdatingLocation ();
			}


		}

		#endregion
		bool Started = false;
		void LocationUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			var loc = e.Locations.Last ();
			if (!Started) {
				Mvx.Resolve<IMotionActivity> ().StartDetection ();
				Started = true;
			}
			var geoLocation = new GeoLocation()
			{
				Speed = loc.Speed,
				Longitude = loc.Coordinate.Longitude,
				Latitude = loc.Coordinate.Latitude,
				Accuracy = loc.HorizontalAccuracy,
				Altitude = loc.Altitude,
				AltitudeAccuracy = loc.VerticalAccuracy
			};
			Location = geoLocation;
			LocationUpdatedEvent (this, new LocationUpdatedEventArgs (geoLocation));
			Mvx.Resolve<IMessengerHub> ().Publish (new LocationUpdatedMessage (this, geoLocation));
			#if DEBUG
			Mvx.Trace(Cirrious.CrossCore.Platform.MvxTraceLevel.Diagnostic,geoLocation.ToString(), string.Empty);
			#endif
		}
	}
}

