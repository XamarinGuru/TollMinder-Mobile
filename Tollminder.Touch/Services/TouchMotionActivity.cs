using System;
using System.Threading.Tasks;
using CoreMotion;
using Foundation;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Touch.Services
{	
	public class TouchMotionActivity : IMotionActivity
	{
		public bool IsBound { get; private set; } = false;
		CMMotionActivityManager _motionActivityManager;
        CMMotionActivityManager MotionActivityManager
        {
            get
            {
                return _motionActivityManager ?? (_motionActivityManager = new CMMotionActivityManager());
            }
        }

		private MotionType _motionType;
		public MotionType MotionType { 
			get { return _motionType; } 
			/*private*/ set { 
				_motionType = value;
				Mvx.Resolve<IMvxMessenger> ().Publish (new MotionMessage (this, value));
			}
		}

		//public bool IsAutomove {
		//	get {
		//		return MotionType == MotionType.Automotive;
		//	}
		//}

		public void StartDetection()
		{			
			if (CMMotionActivityManager.IsActivityAvailable && !IsBound) {
				_motionActivityManager?.StartActivityUpdates (NSOperationQueue.CurrentQueue, (activity) => GetMotionActivity (activity));
				IsBound = true;
			}
		}

		public void StopDetection()
		{			
			if (CMMotionActivityManager.IsActivityAvailable && IsBound) {
				_motionActivityManager?.StopActivityUpdates ();
				IsBound = false;
			}
		}

		void GetMotionActivity (CMMotionActivity activity)
		{			
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

			//Core.Helpers.Log.LogMessage (_motionType.ToString ());
		}
	}
}