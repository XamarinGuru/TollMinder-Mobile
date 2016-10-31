using System.Linq;
using CoreLocation;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Implementation;
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
					StopLocationUpdates();
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
            LocationManager.PausesLocationUpdatesAutomatically = true;
			LocationManager.DesiredAccuracy = CLLocation.AccuracyBest;
            LocationManager.DistanceFilter = SettingsService.DistanceIntervalDefault;
			
            LocationManager.ActivityType = CLActivityType.AutomotiveNavigation;
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
				Log.LogMessage("StartLocationUpdates");
			}
		}

		public virtual void StopLocationUpdates() 
		{
			if (IsBound) {
				if (CLLocationManager.LocationServicesEnabled) {
					CanGetLocation = false;
					LocationManager.LocationsUpdated -= LocationIsUpdated;
					LocationManager.StopUpdatingLocation ();
				}
				IsBound = false;
				Log.LogMessage("StopLocationUpdates");
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

