using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using CoreLocation;
using UIKit;

namespace Tollminder.Touch.Services
{
	public class TouchGeolocationWatcher : IGeoLocationWatcher
	{
		private readonly CLLocationManager _locationManager;
		public TouchGeolocationWatcher ()
		{
			_locationManager = new CLLocationManager ();
			var device = UIDevice.OrientationDidChangeNotification;
			//_locationManager.AllowsBackgroundLocationUpdates = true;
		}

		#region IGeoLocationWatcher implementation

		public GeoLocation GetMyPostion ()
		{
			return new GeoLocation ();
		}

		#endregion
	}
}

