using System;
using CoreMotion;
using CoreLocation;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using System.Collections;
using System.Collections.Generic;
using Foundation;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using MessengerHub;
using Tollminder.Core.Helpers;

namespace Tollminder.Touch.Services
{	
	public class TouchMotionActivity : IMotionActivity
	{
		public bool AuthInProgress { get; set; } = false;
		CMMotionActivityManager _motionActivityManager;

		public TouchMotionActivity ()
		{
			_motionActivityManager = new CMMotionActivityManager (); 
		}

		private MotionType _motionType;
		public MotionType MotionType { 
			get { return _motionType; } 
			private set { 
				if (_motionType == value)
					return;
				else
					_motionType = value;
				Mvx.Resolve<IMessengerHub> ().Publish (new MotionTypeChangedMessage (this, value));
			}
		} 

		public void StopDetection()
		{
			if (CMMotionActivityManager.IsActivityAvailable)
				_motionActivityManager?.StopActivityUpdates ();

		}

		public void StartDetection()
		{			
			if (CMMotionActivityManager.IsActivityAvailable)
				_motionActivityManager?.StartActivityUpdates (NSOperationQueue.CurrentQueue, async (activity) => await GetMotionActivity (activity));
		}

		bool CheckCurrentMotion (MotionType value)
		{
			return 
		}

		Task GetMotionActivity (CMMotionActivity activity)
		{
			return Task.Run (() => {
				if (activity.Walking) {
					MotionType = MotionType.Walking;
				}
				else if (activity.Running) {
					MotionType = MotionType.Running;
				}
				else if (activity.Automotive) {
					MotionType = MotionType.Automotive;
				}
				else if (activity.Stationary || activity.Unknown) {
					MotionType = MotionType.Still;
				}
				#if DEBUG
				Log.LogMessage (_motionType.ToString ());
				#endif
			});
		}
	}
}