using System;
using Android.OS;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Services;
using Tollminder.Droid.Helpers;
using Android.Gms.Common;
using Android.App;

namespace Tollminder.Droid.Handlers
{
	public class MotionClientHanlder : BaseHandler 
	{
		protected new DroidMotionActivity Service {
			get { return base.Service as DroidMotionActivity; }
			set { base.Service = value;	}
		}

		public MotionClientHanlder (DroidMotionActivity service) : base(service)
		{
			Service = service;	
		}

		public override void HandleMessage (Message msg)
		{			
			switch (msg.What) {
			case MotionConstants.StartResolutuon:
				Service.AuthInProgress = true;
				ConnectionResult result = msg.Data.GetParcelable (MotionActivityService.ResultBundleKey) as ConnectionResult;
				result.StartResolutionForResult (Service.Activity, MotionActivityService.ResolutionRequest);
				return;
			default:
				break;
			}
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			Service = null;
		}
	}
}

