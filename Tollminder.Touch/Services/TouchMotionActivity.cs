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
		IGeoLocationWatcher _geoWatcher;
		double _currentSpeed;

		public TouchMotionActivity (IGeoLocationWatcher geoWatcher)
		{
			this._geoWatcher = geoWatcher;
			_motionManager = new CMMotionManager ();			
		}

		const double _minimumSpeed        = 0.3f;
		const double _maximumWalkingSpeed = 1.9f;
		const double _maximumRunningSpeed = 7.5f;
		const double _minimumRunningAcceleration = 3.5f;

		bool _isShaking;
			
		MotionType _previousMotionType;

		CMMotionManager _motionManager;
		CMMotionActivityManager _motionActivityManager;
		NSTimer _timer;


		private MotionType _motionType;
		public MotionType MotionType { 
			get { return _motionType; } 
			private set { 
				_motionType = value;
				Mvx.Resolve<IMessengerHub> ().Publish (new MotionTypeChangedMessage (this, value));
			}
		} 
		public CMAcceleration Acceleration { get; set; }

		public void StopDetection()
		{
			_timer.Invalidate ();
			_timer = null;

			_motionManager.StopAccelerometerUpdates ();
			_motionActivityManager.StopActivityUpdates ();
		}

		public void StartDetection()
		{			
			if (CMMotionActivityManager.IsActivityAvailable) {
				if (_motionActivityManager == null) {		
					_motionActivityManager = new CMMotionActivityManager (); 
				}

				_motionActivityManager.StartActivityUpdates (NSOperationQueue.MainQueue, (activity) => {					
					if (activity.Walking) {
						MotionType = MotionType.Walking;
					} else if (activity.Running) {
						MotionType = MotionType.Running;
					} else if (activity.Automotive) {
						MotionType = MotionType.Automotive;
					} else if (activity.Stationary || activity.Unknown) {
						MotionType = MotionType.Still;
					}
					Log.LogMessage(_motionType.ToString());
				});
			}
		}

		public async void LocationChangedNotification ()
		{
			if (_geoWatcher.Location == null) {
				return;
			}
			_currentSpeed = _geoWatcher.Location.Speed;
			if (_currentSpeed < 0) {
				_currentSpeed = 0;
			}
			await CalculateMotionType ();
			#if DEBUG
			Mvx.Trace (Cirrious.CrossCore.Platform.MvxTraceLevel.Diagnostic, _currentSpeed.ToString(), string.Empty);
			#endif
		}

		Task CalculateMotionType ()
		{
			return Task.Factory.StartNew (() => {
				if (CMMotionActivityManager.IsActivityAvailable) {
					return;
				}

				if (_currentSpeed < _minimumSpeed) {
					_motionType = MotionType.Still;
				} else if (_currentSpeed <= _maximumWalkingSpeed) {
					_motionType = _isShaking ? MotionType.Running : MotionType.Walking;
				} else if (_currentSpeed <= _maximumRunningSpeed) {
					_motionType = _isShaking ? MotionType.Running : MotionType.Automotive;
				} else {
					_motionType = MotionType.Automotive;
				}

				// If type was changed, then call delegate method
				if (_motionType != _previousMotionType) {
					_previousMotionType = _motionType;
				}
			});
		}

		List<CMAcceleration> shakeDataForOneSecond = null;
		float currentFiringTimeInterval = 0.0f;

		void DetectShaking ()
		{
			currentFiringTimeInterval += 0.01f;

			if (currentFiringTimeInterval < 1.0f) {// if one second time intervall not completed yet
				
				// Add current acceleration to array
				if (shakeDataForOneSecond == null) {
					shakeDataForOneSecond = new List<CMAcceleration> ();
				}
				shakeDataForOneSecond.Add (Acceleration);
			} else {
				int shakeCount = 0;
				foreach (CMAcceleration item in shakeDataForOneSecond) {
					double accX_2 = Math.Pow (item.X, 2);
					double accY_2 = Math.Pow (item.Y, 2);
					double accZ_2 = Math.Pow (item.Z, 2);

					double vectorSum = Math.Sqrt (accX_2 + accY_2 + accZ_2);

					if (vectorSum >= _minimumRunningAcceleration) {
						shakeCount++;
					}

				}
				_isShaking = shakeCount > 0;

				shakeDataForOneSecond.Clear ();
				currentFiringTimeInterval = 0.0f;
			}

			#if DEBUG
			#endif

		
		}
	}
}