using System;
using Android.Content;
using Tollminder.Core.Services;
using Tollminder.Droid.Services;
using Android.OS;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.Helpers
{
	public class GeolocationServiceConnection : Java.Lang.Object, IServiceConnection
	{
		private DroidGeolocationWatcher _geoWatcher;

		public GeolocationServiceConnection (DroidGeolocationWatcher geoWatcher)
		{
			_geoWatcher = geoWatcher;			
		}

		public void OnServiceConnected (ComponentName name, Android.OS.IBinder service)
		{
			if (!_geoWatcher.IsBound) {
				_geoWatcher.MessengerService = new Messenger (service);
				_geoWatcher.IsBound = true;
				try {
					DroidMessanging.SendMessage(ServiceConstants.RegisterClient, _geoWatcher.MessengerService, _geoWatcher.Messenger);
				} catch (Exception ex) {
					#if DEBUG
					Log.LogMessage (ex.Message);
					#endif
				}				
			}			
		}

		public void OnServiceDisconnected (ComponentName name)
		{
			_geoWatcher.MessengerService.Dispose ();
			_geoWatcher.MessengerService = null;
			_geoWatcher = null;
		}
		
	}
}

