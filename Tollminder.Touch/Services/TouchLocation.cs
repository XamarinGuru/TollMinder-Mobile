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
		#endregion

		#region Properties
		public CLLocationManager LocationManager {
			get { return _locationManager; }
		}

		public virtual GeoLocation Location { get; set; }
		#endregion

		#region Constructors
		public TouchLocation ()
		{
			_locationManager = new CLLocationManager ();
			SetupLocationService ();
		}
		#endregion

		#region Helper Methods
		protected void SetupLocationService()
		{
			LocationManager.RequestAlwaysAuthorization ();
			LocationManager.RequestWhenInUseAuthorization ();
			LocationManager.DesiredAccuracy = 5;
			if (EnvironmentInfo.IsForIOSNine) {
				LocationManager.AllowsBackgroundLocationUpdates = true;			
			}
		}

		public virtual void StartLocationUpdates() 
		{
			if (CLLocationManager.LocationServicesEnabled) {
				LocationManager.LocationsUpdated += LocationIsUpdated;
				LocationManager.StartUpdatingLocation ();
			}
		}

		public virtual void StoptLocationUpdates() 
		{
			if (CLLocationManager.LocationServicesEnabled) {
				LocationManager.LocationsUpdated -= LocationIsUpdated;
				LocationManager.StopUpdatingLocation ();
			}
		}

		protected virtual void LocationIsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			var loc = e.Locations.Last ();
			if (loc != null) {				
				Location = loc.GetGeoLocationFromCLLocation ();
			}
		}
		#endregion
	}
}

