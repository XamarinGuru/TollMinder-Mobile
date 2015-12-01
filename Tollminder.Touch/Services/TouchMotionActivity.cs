using System;
using CoreMotion;
using CoreLocation;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Touch.Services
{
	public class TouchMotionActivity : IMotionActivity
	{
		double _minimumSpeed        = 0.3f;
		double _maximumWalkingSpeed = 1.9f;
		double _maximumRunningSpeed = 7.5f;
		double _minimumRunningAcceleration = 3.5f;

		CLLocation _currentLocation;
		MotionType _previousMotionType;

		CMMotionManager _motionManager;
		CMMotionActivityManager _motionActivityManager;

		public bool UseM7IfAvailable { get; set; }
		MotionType _motionType;
		public MotionType MotionType { get; }

		public void StartDetection()
		{
			
		}

		public void StopDetection()
		{
//			[[SOLocationManager sharedInstance] start];

//			self.shakeDetectingTimer = [NSTimer scheduledTimerWithTimeInterval:0.01f target:self selector:@selector(detectShaking) userInfo:Nil repeats:YES];

//			_motionManager.StartAccelerometerUpdates(
//				_acceleration = accelerometerData.acceleration;
//				[self calculateMotionType];
//				dispatch_async(dispatch_get_main_queue(), ^{
//					
//					if (self.delegate && [self.delegate respondsToSelector:@selector(motionDetector:accelerationChanged:)]) {
//						[self.delegate motionDetector:self accelerationChanged:self.acceleration];
//					}
//					#pragma GCC diagnostic pop
//
//
//					if (self.accelerationChangedBlock) {
//						self.accelerationChangedBlock (self.acceleration);
//					}
//				});
//			}];

//			if (UseM7IfAvailable && CMMotionActivityManager.IsActivityAvailable) {
				if (_motionActivityManager == null) {
					_motionActivityManager = new CMMotionActivityManager (); 
				}

			_motionActivityManager.StartActivityUpdates (Foundation.NSOperationQueue.CurrentQueue, async (activity) => {
					if (activity.Walking) {
					_motionType = MotionType.Walking;
					} else if (activity.Running) {
					_motionType = MotionType.Running;
					} else if (activity.Automotive) {
					_motionType = MotionType.Automotive;
					} else if (activity.Stationary || activity.Unknown) {
					_motionType = MotionType.Still;
					}
				});
//			}
		}
	}
}

