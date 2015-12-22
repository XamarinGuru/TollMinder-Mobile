using System;
using Android.OS;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Helpers;

namespace Tollminder.Droid.Handlers
{
	public class MotionServiceHanlder : BaseHandler 
	{
		protected new MotionActivityService Service {
			get { return base.Service as MotionActivityService; }
			set { base.Service = value;	}
		}

		public MotionServiceHanlder (MotionActivityService service) : base(service)
		{
			Service = service;	
		}

		public override void HandleMessage (Message msg)
		{	
			switch (msg.What) {
			case MotionConstants.RegisterMessenger:
				Service.MessengerClient = msg.ReplyTo;
				return;
			case MotionConstants.UnRegisterMessenger:
				if (Service.MessengerClient != null) {					
					Service.MessengerClient.Dispose ();
				}
				return;
			case MotionConstants.ConnectToGoogleFit:
				Service.Connect ();
				return;
			}
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			Service = null;
		}
	}
}

