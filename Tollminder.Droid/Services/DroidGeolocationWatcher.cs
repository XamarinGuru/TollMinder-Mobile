using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Android.Content;
using Tollminder.Droid.Helpers;
using Android.OS;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.Services
{
	public class DroidGeolocationWatcher : Java.Lang.Object, IGeoLocationWatcher
	{	
		private readonly Context _applicationContext;
		private readonly Intent _serviceIntent;
		public bool IsBound { get; set; }
		private GeolocationServiceConnection _serviceConnecton;
		public Messenger Messenger { get; set; }
		public Messenger MessengerService { get; set; }
		private GeolocationClientHandler _clientHandler;

		public DroidGeolocationWatcher ()
		{
			_applicationContext = Mvx.Resolve<IMvxAndroidCurrentTopActivity> ().Activity.ApplicationContext;
			_serviceIntent = new Intent (_applicationContext, typeof(DroidGeolocationTracker));
			_clientHandler = new GeolocationClientHandler (this);
			Messenger = new Messenger (_clientHandler);
		}	

		#region IGeoLocationWatcher implementation

		public event EventHandler<LocationUpdatedEventArgs> LocationUpdatedEvent;

		public GeoLocation Location {
			get;
			set;
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
					Message msg = Message.Obtain(null,
						ServiceConstants.UnregisterClient);
					msg.ReplyTo = Messenger;
					MessengerService.Send(msg);
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