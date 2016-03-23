using System;
using System.Linq;
using CoreLocation;
using Foundation;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
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
			Mvx.Resolve<IMvxMessenger> ().SubscribeOnThreadPoolThread<AppInBackgroundMessage> ((obj) => {
				LocationManager.StopUpdatingLocation ();
				if (!Mvx.Resolve<TouchPlatform> ().IsAppInForeground) {
					//LocationManager.DesiredAccuracy = CLLocation.AccuracyBest;
					LocationManager.DistanceFilter = 400;
				} else {
					//LocationManager.DesiredAccuracy = CLLocation.AccuracyBest;
					LocationManager.DistanceFilter = CLLocationDistance.FilterNone; 
				}
				LocationManager.StartUpdatingLocation ();
			});
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
			LocationManager.PausesLocationUpdatesAutomatically = false;
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
					LocationManager.LocationsUpdated += LocationIsUpdated;
					LocationManager.StartUpdatingLocation ();
				}
				IsBound = true;
			}
		}


		public virtual void StoptLocationUpdates ()
		{
			if (IsBound) {
				if (CLLocationManager.LocationServicesEnabled) {
					LocationManager.LocationsUpdated -= LocationIsUpdated;
					LocationManager.DeferredUpdatesFinished -= DeferredUpdatesFinished;
					LocationManager.StopUpdatingLocation ();
				}
				IsBound = false;
			}
		}

		private bool _defferedUpdates = true;

		protected virtual void DeferredUpdatesFinished (object sender, NSErrorEventArgs e)
		{
			_defferedUpdates = false;
		}

		protected virtual void LocationIsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			var loc = e.Locations.Last ();
			if (loc != null) {
				Location = loc.GetGeoLocationFromCLLocation ();
				if (!_defferedUpdates && !Mvx.Resolve<TouchPlatform> ().IsAppInForeground) {
					_defferedUpdates = true;
					LocationManager.AllowDeferredLocationUpdatesUntil (400, 20);
				}
			}
		}
		#endregion
	}
}

