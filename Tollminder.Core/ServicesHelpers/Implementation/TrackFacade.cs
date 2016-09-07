using System;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Models.Statuses;
using Tollminder.Core.Services;

namespace Tollminder.Core.ServicesHelpers.Implementation
{
	public class TrackFacade : ITrackFacade
	{
		public const double WaypointDistanceRequired = 0.01;

		#region Services

		private readonly IGeoLocationWatcher _geoWatcher;
		private readonly IMotionActivity _activity;
		private readonly ITextToSpeechService _textToSpeech;
		private readonly IMvxMessenger _messenger;
		private readonly IBatteryDrainService _batteryDrainService;

		private MvxSubscriptionToken _token;

		private object lockOebject = new object ();

		#endregion

		#region Constructor

		public TrackFacade ()
		{
			this._textToSpeech = Mvx.Resolve<ITextToSpeechService> ();
			this._messenger = Mvx.Resolve<IMvxMessenger> ();
			this._geoWatcher = Mvx.Resolve<IGeoLocationWatcher> ();
			this._activity = Mvx.Resolve<IMotionActivity> ();
			this._batteryDrainService = Mvx.Resolve<IBatteryDrainService>();
		}

		#endregion

		#region Properties

		private bool _isBound;

		#endregion

		public virtual async void StartServices ()
		{			
			bool isGranted = await Mvx.Resolve<IPermissionsService> ().CheckPermissionsAccesGrantedAsync ();
			if (!_isBound & isGranted) {
				#if DEBUG 
				Log.LogMessage (string.Format ("THE SEVICES HAS STARTED AT {0}", DateTime.Now));
				#endif
				_textToSpeech.IsEnabled = true;
				_geoWatcher.StartGeolocationWatcher ();
				_token = _messenger.SubscribeOnThreadPoolThread<LocationMessage> (x => CheckTrackStatus ());
				_activity.StartDetection ();			
				_isBound = true;
			}	

		}


		public virtual void StopServices ()
		{	
			if (_isBound) {
				#if DEBUG 
				Log.LogMessage (string.Format ("THE SEVICES HAS STOPPED AT {0}", DateTime.Now));
				#endif
				_geoWatcher.StopGeolocationWatcher ();
				_token?.Dispose ();
				_activity.StopDetection ();
				_isBound = false;
			}
		}


		public TollGeolocationStatus TollStatus { get; set; } = TollGeolocationStatus.NotOnTollRoad;

		protected virtual void CheckTrackStatus ()
		{
			lock(lockOebject)
			{
				BaseStatus statusObject = StatusesFactory.GetStatus (TollStatus);

				if (TollStatus == TollGeolocationStatus.NotOnTollRoad)
					_batteryDrainService.NeedStopGpsTracking();

				Log.LogMessage (TollStatus.ToString ());
				TollStatus = statusObject.CheckStatus ();

			}
		}
	}
}