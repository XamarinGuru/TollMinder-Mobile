using System;
using Android.OS;
using Tollminder.Droid.Services;

namespace Tollminder.Droid.Helpers
{
	public class GeolocationClientHandler : Handler
	{
		DroidGeolocationWatcher _watcher;

		public DroidGeolocationWatcher Watcher {
			get { return _watcher; }
			set { _watcher = value;	}
		}

		public GeolocationClientHandler (DroidGeolocationWatcher watcher)
		{
			this._watcher = watcher;			
		}

		public override void HandleMessage (Message msg)
		{
			switch (msg.What) {
			case ServiceConstants.ServicePushLocations:
				_watcher.Location = msg.Data.GetGeolocationFromAndroidLocation ();
				break;	
			}
		}

		protected override void Dispose (bool disposing)
		{
			_watcher = null;
			base.Dispose (disposing);
		}
	}
}

