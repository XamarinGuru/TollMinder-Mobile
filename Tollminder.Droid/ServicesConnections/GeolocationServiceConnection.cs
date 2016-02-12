using System;
using Android.Content;
using Tollminder.Core.Services;
using Tollminder.Droid.Services;
using Android.OS;
using Tollminder.Core.Helpers;
using Tollminder.Droid.Helpers;
using Tollminder.Droid.Inerfaces;

namespace Tollminder.Droid.ServicesConnections
{
	public class GeolocationServiceConnection : BaseServiceConnection
	{		

		public override void OnServiceConnected (ComponentName name, IBinder service)
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

		public override void OnServiceDisconnected (ComponentName name)
		{				
			base.OnServiceDisconnected (name);
		}
	}
}