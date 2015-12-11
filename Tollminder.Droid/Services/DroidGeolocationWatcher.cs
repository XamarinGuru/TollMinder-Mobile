using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Android.Content;
using Tollminder.Droid.Helpers;
using Android.OS;
using Tollminder.Core.Helpers;
using MessengerHub;

namespace Tollminder.Droid.Services
{
	public class DroidGeolocationWatcher : Java.Lang.Object, IGeoLocationWatcher
	{	
		private readonly Context _applicationContext;
		private readonly Intent _serviceIntent;
		private GeolocationServiceConnection _serviceConnecton;
		private readonly GeolocationClientHandler _clientHandler;
		private readonly IMessengerHub _messengerHub;
		private readonly IPlatform _platform;
		private readonly INotificationSender _notficantioSender;

		public bool IsBound { get; set; }
		public Messenger Messenger { get; set; }
		public Messenger MessengerService { get; set; }

		public DroidGeolocationWatcher ()
		{
			_applicationContext = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity.ApplicationContext;
			_serviceIntent = new Intent (_applicationContext, typeof(DroidGeolocationTracker));
			_clientHandler = new GeolocationClientHandler (this);
			_messengerHub = Mvx.Resolve<IMessengerHub> ();
			_platform = Mvx.Resolve<IPlatform> ();
			_notficantioSender = Mvx.Resolve<INotificationSender> ();

			Messenger = new Messenger (_clientHandler);
		}	

		#region IGeoLocationWatcher implementation
		#pragma warning disable 67
		public event EventHandler<LocationUpdatedEventArgs> LocationUpdatedEvent;
		#pragma warning restore 67
		GeoLocation _location;
		public GeoLocation Location {
			get {
				return _location;
			}
			set {
				_location = value;
				if (_platform.IsAppInForeground) {
					_messengerHub.Publish (new LocationUpdatedMessage (this, _location));
				} else {
					_notficantioSender.SendLocalNotification ("Exit from geofence", _location.ToString ());
				}
			}
		}

		public void StartGeolocationWatcher ()
		{
			_serviceConnecton = new GeolocationServiceConnection (this);
			_applicationContext.BindService (_serviceIntent, _serviceConnecton, Bind.AutoCreate);

		}

		public void StopGeolocationWatcher ()
		{
			if (IsBound & MessengerService != null) {
				try {
					DroidMessanging.SendMessage(ServiceConstants.UnregisterClient,MessengerService,Messenger);
				} catch (Exception ex) {
					#if DEBUG
					Log.LogMessage(ex.Message);
					#endif
				}				
				_applicationContext.UnbindService (_serviceConnecton);
				IsBound = false;
				_clientHandler.Dispose ();
			}
		}
		#endregion
	}
}