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
			
		}
		public virtual void OnServiceDisconnected (ComponentName name)
		{
			Messenger = null;
		}
	}
}

