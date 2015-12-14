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
		private readonly TouchGeoFence _geofence;

		GeoLocation _location;
		public GeoLocation Location {
			get { return _location;	}
			set {
				_location = value;
				Mvx.Resolve<IMessengerHub> ().Publish (new LocationUpdatedMessage (this, _location));
			}
		}

		public TouchGeolocationWatcher ()
		{
			_geofence = new TouchGeoFence ();
		}

		#region IGeoLocationWatcher implementation

		public void StartGeolocationWatcher()
		{	
			_geofence.StartGeofenceService ();
		}

		public void StopGeolocationWatcher ()
		{
			_geofence.StopGeofenceService ();
		}

		#endregion
	
	}
}