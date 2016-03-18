using System;
using System.Linq;
using CoreLocation;
using Foundation;
using Tollminder.Core.Models;
using Tollminder.Touch.Helpers;

namespace Tollminder.Touch.Services
{
	public class TouchDeferedLocation : CLLocationManagerDelegate
	{
		#region Private Fields
		private readonly CLLocationManager _locationManager;
		#endregion

		#region Properties
		public CLLocationManager LocationManager {
			get { return _locationManager; }
		}

		private bool IsBound { get; set; }

		public virtual GeoLocation Location { get; set; }
		#endregion

		#region Constructors
		public TouchDeferedLocation ()
		{
			_locationManager = new CLLocationManager ();
			SetupLocationService ();
		}
		#endregion

		#region Helper Methods
		protected void SetupLocationService ()
		{
			LocationManager.RequestAlwaysAuthorization ();
			LocationManager.RequestWhenInUseAuthorization ();
			LocationManager.DesiredAccuracy = CLLocation.AccuracyBest;
			LocationManager.DistanceFilter = CLLocationDistance.FilterNone;
			LocationManager.Delegate = this;
			if (EnvironmentInfo.IsForIOSNine) {
				LocationManager.AllowsBackgroundLocationUpdates = true;
			}
		}

		protected void DestroyLocationService ()
		{
			
		}

		public virtual void StartLocationUpdates ()
		{
			if (!IsBound) {
				if (CLLocationManager.LocationServicesEnabled) {
					LocationManager.DeferredUpdatesFinished += DeferredUpdatesFinished;
					LocationManager.AllowDeferredLocationUpdatesUntil (CLLocation.AccuracyThreeKilometers, 20);
					LocationManager.LocationsUpdated -= LocationIsUpdated;
				}
				IsBound = true;
			}
		}


		public virtual void StoptLocationUpdates ()
		{
			if (IsBound) {
				if (CLLocationManager.LocationServicesEnabled) {
					LocationManager.LocationsUpdated -= LocationIsUpdated;
				}
				IsBound = false;
			}
		}

		protected virtual void DeferredUpdatesFinished (object sender, NSErrorEventArgs e)
		{
			
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

