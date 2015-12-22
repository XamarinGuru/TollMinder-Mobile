using System;
using Android.Content;
using Tollminder.Droid.Inerfaces;
using Tollminder.Droid.Helpers;
using Android.OS;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.ServicesConnections
{
	public class BaseServiceConnection : Java.Lang.Object, IServiceConnection
	{
		public IDroidMessenger Messenger { get; set; }

		public virtual void OnServiceConnected (ComponentName name, IBinder service)
		{
			Messenger.MessengerService = new Messenger (service);
			try {
				DroidMessanging.SendMessage(ServiceConstants.RegisterClient, Messenger.MessengerService, Messenger.Messenger);
			} catch (Exception ex) {
				#if DEBUG
				Log.LogMessage (ex.Message);
				#endif
			}						
		}

		public virtual void OnServiceDisconnected (ComponentName name)
		{			
			Messenger = null;
		}
	}
}

