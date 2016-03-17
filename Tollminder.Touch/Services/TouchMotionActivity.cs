﻿using System;
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

		public bool IsAutomove {
			get {
				return MotionType == MotionType.Automotive;
			}
		}

		public void StopDetection()
		{			
			if (CMMotionActivityManager.IsActivityAvailable && IsBound) {
				_motionActivityManager?.StopActivityUpdates ();
				IsBound = false;
			}
		}

		public void StartDetection()
		{			
			if (CMMotionActivityManager.IsActivityAvailable && !IsBound) {
				_motionActivityManager?.StartActivityUpdates (NSOperationQueue.MainQueue, (activity) => GetMotionActivity (activity));
				IsBound = true;
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
			#if RELEASE
			Tollminder.Core.Helpers.Log.LogMessage (_motionType.ToString ());
			#endif

		}
	}
}