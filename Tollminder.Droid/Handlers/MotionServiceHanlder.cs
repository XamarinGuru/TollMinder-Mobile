using System;
using Android.OS;
using Tollminder.Droid.AndroidServices;

namespace Tollminder.Droid.Handlers
{
	public class MotionServiceHanlder : BaseHandler 
	{
		public MotionServiceHanlder (MotionActivityService service) : base(service)
		{
			Service = service;	
		}

		public override void HandleMessage (Message msg)
		{
			base.HandleMessage (msg);
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			Service = null;
		}
	}
}

