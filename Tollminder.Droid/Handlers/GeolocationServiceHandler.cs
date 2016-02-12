using System;
using Android.OS;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Helpers;

namespace Tollminder.Droid.Handlers
{
	public class GeolocationServiceHandler : BaseHandler
	{
		protected new GeolocationService Service {
			get { return base.Service as GeolocationService; }
			set { base.Service = value;	}
		}

		public GeolocationServiceHandler (GeolocationService tracker) : base(tracker)
		{
			Service = tracker;
			
		}
		public override void HandleMessage (Message msg)
		{
			switch (msg.What) {
			case ServiceConstants.RegisterClient:
				Service.MessengerClient = msg.ReplyTo;
				return;
			case ServiceConstants.UnregisterClient:
				if (Service.MessengerClient != null) {					
					Service.MessengerClient.Dispose ();
				}
				return;		
			case ServiceConstants.GeoFenceEnabled:
				(Service as GeofenceService).GeofenceEnabled = msg.Data.GetIsEnabled ();
				return;
			case ServiceConstants.StartLocation:
				Service.StartLocationUpdate ();
				return;	
			case ServiceConstants.StopLocation:
				Service.StopLocationUpdate ();
				return;		
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