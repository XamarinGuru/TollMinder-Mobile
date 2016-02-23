using System.Linq;
using CoreLocation;
using Tollminder.Core.Models;
using Tollminder.Touch.Helpers;

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

		private	bool IsBound { get; set; }

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
			if (!IsBound) {
				if (CLLocationManager.LocationServicesEnabled) {
					LocationManager.LocationsUpdated += LocationIsUpdated;
					LocationManager.StartUpdatingLocation ();
				}
				IsBound = true;
			}
		}

		public virtual void StoptLocationUpdates() 
		{
			if (IsBound) {
				if (CLLocationManager.LocationServicesEnabled) {
					LocationManager.LocationsUpdated -= LocationIsUpdated;
					LocationManager.StopUpdatingLocation ();
				}
				IsBound = false;
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

