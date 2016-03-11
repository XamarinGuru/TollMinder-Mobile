using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Tollminder.Droid.AndroidServices;
using Tollminder.Droid.Handlers;
using Tollminder.Droid.ServicesConnections;
using MvvmCross.Plugins.Messenger;

namespace Tollminder.Droid.Services
{
	public class DroidMotionActivity : AndroidServiceWithServiceConnection<MotionActivityService,MotionClientHanlder, MotionServiceConnection> , IMotionActivity
	{
		public bool IsBound { get; private set; } = false;
		#region IMotionActivity implementation

		public void StartDetection ()
		{
			if (!IsBound) {
				Start ();
				IsBound = true;				
			}	
		}

		public void StopDetection ()
		{
			if (IsBound & MessengerService != null) {				
				Stop ();
				IsBound = false;
			}
		}

		private MotionType _motionType;
		public virtual MotionType MotionType {
			get { return _motionType; }
			set {
				_motionType = value;
				Mvx.Resolve<IMvxMessenger> ().Publish (new MotionMessage (this, value));
			}
		}

		public bool IsAutomove { get { return _motionType == MotionType.Automotive; } }
		#endregion
	}
}

