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

		#region Private Fields

		#endregion

		#region Constructors
		public TouchGeoFence () : base ()
		{
			SetupGeofenceService ();	
		}
		#endregion

		#region Properties
		public override GeoLocation Location {
			get {
				return base.Location;
			}
			set {
				base.Location = value;
				UpdateGeofenceRegion (Location);
			}
		}
		#endregion

		#region Methods
		public void UpdateGeofenceRegion(GeoLocation location)
		{
			RemoveAllRegions ();
			CLCircularRegion region = new CLCircularRegion (new CLLocationCoordinate2D (location.Latitude, location.Longitude), GeoFenceRadius, TollMinder);
			region.NotifyOnEntry = true;
			region.NotifyOnExit = true;
			LocationManager.StartMonitoring (region);
		}

		public void RemoveAllRegions ()
		{
			foreach (CLRegion item in LocationManager.MonitoredRegions) {
				LocationManager.StopMonitoring (item);
			}
		}

		private void SetupGeofenceService ()
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

		public void StartGeofenceService() 
		{
			StartLocationUpdates ();
		}

		public void StopGeofenceService ()
		{
			StoptLocationUpdates ();
			if (CLLocationManager.IsMonitoringAvailable (typeof(CLCircularRegion))) {
				RemoveAllRegions ();
				LocationManager.DidStartMonitoringForRegion -= StartedMonitorRegionHandler;
				LocationManager.RegionEntered -= RegionEnteredHandler;
				LocationManager.RegionLeft -= RegionLeftHandler;
			}
			else {
				Log.LogMessage ("This app requires region monitoring, which is unavailable on this device");
			}
		}

		private void StartedMonitorRegionHandler (object sender, CLRegionEventArgs e)
		{
			Log.LogMessage (string.Format ("{0} {1} START MONITORING", e.Region.Center.Latitude , e.Region.Center.Longitude));
		}

		private void RegionEnteredHandler (object sender, CLRegionEventArgs e)
		{
			Log.LogMessage (string.Format ("{0} {1} --- ENTERED", e.Region.Center.Latitude , e.Region.Center.Longitude));
		}

		private void RegionLeftHandler (object sender, CLRegionEventArgs e)
		{
			Log.LogMessage (string.Format ("{0} {1} --- LEFT", e.Region.Center.Latitude , e.Region.Center.Longitude));
			StartLocationUpdates ();
		}
		#endregion
	}
}

