using System;
using Android.OS;
using Tollminder.Droid.Services;

namespace Tollminder.Droid.Helpers
{
	public class GeolocationServiceHandler : Handler
	{
		private DroidGeolocationTracker _tracker;
		public DroidGeolocationTracker Tracker {
			get { return _tracker;	}
			set { _tracker = value;	}
		}

		public GeolocationServiceHandler (DroidGeolocationTracker tracker)
		{
			this._tracker = tracker;
			
		}
		public override void HandleMessage (Message msg)
		{
			switch (msg.What) {
			case ServiceConstants.RegisterClient:
				Tracker.MessengerClient = msg.ReplyTo;
				return;
			case ServiceConstants.UnregisterClient:
				if (Tracker.MessengerClient != null) {					
					Tracker.MessengerClient.Dispose ();
				}
				return;										
			}
		}
		protected override void Dispose (bool disposing)
		{
			_tracker = null;
			base.Dispose (disposing);
		}
	}
}