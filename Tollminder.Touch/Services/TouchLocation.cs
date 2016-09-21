using System.Linq;
using CoreLocation;
using Tollminder.Core.Models;
using Tollminder.Touch.Helpers;

namespace Tollminder.Touch.Services
{	
	public class TouchLocation
	{
		readonly CLLocationManager _locationManager;

		public CLLocationManager LocationManager 
		{
			get { return _locationManager; }
		}

		bool IsBound { get; set; }
		bool CanGetLocation { get; set; }

		bool _canGetNewLocation = true;
		private bool CanGetNewLocation 
		{ 
			get 
			{
				return _canGetNewLocation;
			}
			set
			{
				_canGetNewLocation = value;
				if (value)
					StartLocationUpdates();
				else
					StoptLocationUpdates();
			}
		}

		public virtual GeoLocation Location { get; set; }

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
			LocationManager.PausesLocationUpdatesAutomatically = false;
			LocationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			LocationManager.DistanceFilter = 400;
			//TODO: Need to be tested.
			//LocationManager.ActivityType = CLActivityType.AutomotiveNavigation;
			if (EnvironmentInfo.IsForIOSNine) {
				LocationManager.AllowsBackgroundLocationUpdates = true;			
			}
		}

		public virtual void StartLocationUpdates() 
		{
			if (!IsBound) {
				if (CLLocationManager.LocationServicesEnabled) {
					CanGetLocation = true;
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
					CanGetLocation = false;
					LocationManager.LocationsUpdated -= LocationIsUpdated;
					LocationManager.StopUpdatingLocation ();
				}
				IsBound = false;
			}
		}

		protected virtual void LocationIsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			var loc = e.Locations.Last ();
			if (loc != null && CanGetLocation) {				
				Location = loc.GetGeoLocationFromCLLocation ();
			}
		}
		#endregion
	}
}

