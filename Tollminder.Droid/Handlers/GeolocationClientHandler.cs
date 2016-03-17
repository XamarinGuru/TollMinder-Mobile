using System;
using Android.OS;
using Tollminder.Droid.Services;
using Tollminder.Droid.Helpers;

namespace Tollminder.Droid.Handlers
{
	public class GeolocationClientHandler : BaseHandler
	{
		protected new DroidGeolocationWatcher Service {
			get { return base.Service as DroidGeolocationWatcher; }
			set { base.Service = value;	}
		}

		public GeolocationClientHandler (DroidGeolocationWatcher watcher) : base(watcher)
		{
			Service = watcher;			
		}

		public override void HandleMessage (Message msg)
		{
			switch (msg.What) {
			case ServiceConstants.ServicePushLocations:
				if (Service != null) 
					Service.Location = msg.Data.GetGeolocationFromAndroidLocation ();
					break;	
			}
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {				
				Service = null;
			}
			base.Dispose (disposing);
		}
	}
}

