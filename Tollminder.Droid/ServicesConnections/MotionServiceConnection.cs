using System;
using Android.Content;
using Android.OS;
using Tollminder.Droid.Helpers;
using Tollminder.Core.Helpers;

namespace Tollminder.Droid.ServicesConnections
{
	public class MotionServiceConnection : BaseServiceConnection
	{
		public override void OnServiceConnected (ComponentName name, Android.OS.IBinder service)
		{
			Messenger.MessengerService = new Messenger (service);
			try {
				DroidMessanging.SendMessage(MotionConstants.RegisterMessenger, Messenger.MessengerService, Messenger.Messenger);
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