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

		private MotionType _motionType;
		public virtual MotionType MotionType {
			get { return _motionType; }
			set {
				_motionType = value;
				Mvx.Resolve<IMvxMessenger> ().Publish (new MotionMessage (this, value));
			}
		}

		#endregion
	}
}

