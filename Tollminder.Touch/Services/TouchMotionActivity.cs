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
				Mvx.Resolve<IMvxMessenger> ().Publish (new MotionMessage (this, value));
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
				_motionActivityManager?.StartActivityUpdates (NSOperationQueue.MainQueue, (activity) => GetMotionActivity (activity));
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
			#if DEBUG
			Tollminder.Core.Helpers.Log.LogMessage (_motionType.ToString ());
			#endif

		}
	}
}