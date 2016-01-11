using System;
using Foundation;
using CoreLocation;
using System.Linq;
using Cirrious.CrossCore;
using Tollminder.Core.Services;
using Tollminder.Touch.Helpers;
using Tollminder.Core.Models;

namespace Tollminder.Touch.Services
{	
	public class TouchLocation
	{
		#region Private Fields
		private readonly CLLocationManager _locationManager;
		private GeoLocation _location;
		#endregion

		#region Properties
		public CLLocationManager LocationManager {
			get { return _locationManager; }
		}

		public virtual GeoLocation Location {
			get { return _location;	}
			set {
				_location = value;
				StoptLocationUpdates ();
			}
		}
		#endregion

		#region Constructors
		public TouchLocation ()
		{
			_locationManager = new CLLocationManager ();
			SetupLocationService ();
		}
		#endregion

		#region Helper Methods
		private void SetupLocationService()
		{
			_locationManager.RequestAlwaysAuthorization ();
			_locationManager.RequestWhenInUseAuthorization ();
			if (EnvironmentInfo.IsForIOSNine) {
				_locationManager.AllowsBackgroundLocationUpdates = true;				
			}
		}

		public virtual void StartLocationUpdates() 
		{
			if (CLLocationManager.LocationServicesEnabled) {
				_locationManager.DesiredAccuracy = 5;
				_locationManager.LocationsUpdated += LocationIsUpdated;
				_locationManager.StartUpdatingLocation ();
			}
		}

		public virtual void StoptLocationUpdates() 
		{
			if (CLLocationManager.LocationServicesEnabled) {
				_locationManager.LocationsUpdated -= LocationIsUpdated;
				_locationManager.StopUpdatingLocation ();
			}
		}

		private void LocationIsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			var loc = e.Locations.Last ();
			if (loc != null) {				
				Location = loc.GetGeoLocationFromCLLocation ();
			}
		}
		#endregion
	}
}

