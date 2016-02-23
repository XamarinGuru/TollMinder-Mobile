using System;
using Foundation;
using CoreLocation;
using Tollminder.Core.Models;
using Tollminder.Core.Helpers;

namespace Tollminder.Touch.Services
{	
	[Register("TouchGeoFence")]
	public class TouchGeoFence : TouchLocation
	{
		#region Constants
		public const double GeoFenceRadius = 200d;
		public const string TollMinder = "TollMinder";
		#endregion

		#region Properties
		bool _geofenceEnabled = true;
		public bool GeofenceEnabled {
			get {
				return _geofenceEnabled;
			}
			set {
				_geofenceEnabled = value;
				EnableGeofence ();
			}
		}
		public override GeoLocation Location {
			get {
				return base.Location;
			}
			set {
				base.Location = value;
				Log.LogMessage ("NEW LOCATION");
				if (GeofenceEnabled) {
					StoptLocationUpdates ();
					StartMonitoringRegion ();					
				}
			}
		}
		#endregion

		#region Methods
		public virtual void StartMonitoringRegion()
		{
			StopMonitoringRegion ();
			CLCircularRegion region = new CLCircularRegion (new CLLocationCoordinate2D (Location.Latitude, Location.Longitude), GeoFenceRadius, TollMinder);
			region.NotifyOnEntry = true;
			region.NotifyOnExit = true;
			LocationManager.StartMonitoring (region);
		}

		public virtual void StopMonitoringRegion ()
		{
			Log.LogMessage ("STOP MONITORING ALL REGIONS");
			foreach (CLRegion item in LocationManager.MonitoredRegions) {
				LocationManager.StopMonitoring (item);
			}
		}

		protected virtual void SetupGeofenceService ()
		{
			if (CLLocationManager.IsMonitoringAvailable (typeof(CLCircularRegion))) {
				LocationManager.DidStartMonitoringForRegion += StartedMonitorRegionHandler;
				LocationManager.RegionEntered += RegionEnteredHandler;
				LocationManager.RegionLeft += RegionLeftHandler;
			}
			else {
				Log.LogMessage ("This app requires region monitoring, which is unavailable on this device");
			}
		}

		protected virtual void DestroyGeofenceService ()
		{
			if (CLLocationManager.IsMonitoringAvailable (typeof(CLCircularRegion))) {
				StopMonitoringRegion ();
				LocationManager.DidStartMonitoringForRegion -= StartedMonitorRegionHandler;
				LocationManager.RegionEntered -= RegionEnteredHandler;
				LocationManager.RegionLeft -= RegionLeftHandler;
			}
			else {
				Log.LogMessage ("This app requires region monitoring, which is unavailable on this device");
			}
		}

		public virtual void StartGeofenceService() 
		{
			SetupGeofenceService ();	
			StartLocationUpdates ();
		}

		public virtual void StopGeofenceService ()
		{			
			DestroyGeofenceService ();
			StoptLocationUpdates ();
		}

		protected virtual void StartedMonitorRegionHandler (object sender, CLRegionEventArgs e)
		{
			#if DEBUG
			Log.LogMessage (string.Format ("{0} {1} START MONITORING", e.Region.Center.Latitude , e.Region.Center.Longitude));
			#endif
		}

		protected virtual void RegionEnteredHandler (object sender, CLRegionEventArgs e)
		{   
			#if DEBUG
			Log.LogMessage (string.Format ("{0} {1} --- ENTERED", e.Region.Center.Latitude , e.Region.Center.Longitude));
			#endif
		}

		protected virtual void RegionLeftHandler (object sender, CLRegionEventArgs e)
		{
			#if DEBUG
			Log.LogMessage (string.Format ("{0} {1} --- LEFT", e.Region.Center.Latitude , e.Region.Center.Longitude));
			#endif
			StartLocationUpdates ();
		}

		private void EnableGeofence ()
		{
			if (!_geofenceEnabled)
				StopMonitoringRegion ();				
		}
		#endregion
	}
}

