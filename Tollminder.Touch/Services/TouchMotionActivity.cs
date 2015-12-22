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
		IGeoLocationWatcher _geoWatcher;
		double _currentSpeed;

		public TouchMotionActivity (IGeoLocationWatcher geoWatcher)
		{
			this._geoWatcher = geoWatcher;			
		}
		CMMotionActivityManager _motionActivityManager;

		private MotionType _motionType;
		public MotionType MotionType { 
			get { return _motionType; } 
			private set { 
				_motionType = value;
				Mvx.Resolve<ITextToSpeechService> ().Speak (value.ToString ());
				Mvx.Resolve<IMessengerHub> ().Publish (new MotionTypeChangedMessage (this, value));
			}
		} 
		public CMAcceleration Acceleration { get; set; }

		public void StopDetection()
		{
			_motionActivityManager.StopActivityUpdates ();
		}

		public void StartDetection()
		{			
			if (CMMotionActivityManager.IsActivityAvailable) {
				if (_motionActivityManager == null) {		
					_motionActivityManager = new CMMotionActivityManager (); 
				}

				_motionActivityManager.StartActivityUpdates (NSOperationQueue.CurrentQueue, async (activity) => await GetMotionActivity (activity));
			}
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