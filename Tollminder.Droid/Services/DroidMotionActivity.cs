using System;
using Tollminder.Core.Services;
using Tollminder.Core.Models;
using Android.Content;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Handlers;
using Tollminder.Droid.ServicesConnections;

namespace Tollminder.Droid.Services
{
	public class DroidMotionActivity : AndroidServiceWithServiceConnection<MotionActivityService,MotionClientHanlder, MotionServiceConnection> , IMotionActivity
	{
		public bool AuthInProgress { get; set; } = false;
		#region IMotionActivity implementation

		public void StartDetection ()
		{
			Start ();	
		}

		public void StopDetection ()
		{
			Stop ();
		}

		public MotionType MotionType {
			get {
				return MotionType.Still;
			}
		}

		#endregion
	}
}

