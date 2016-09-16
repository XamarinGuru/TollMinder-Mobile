using System;
using System.Threading.Tasks;
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

	 	readonly IGeoLocationWatcher _geoWatcher;
		readonly IMotionActivity _activity;
		readonly ITextToSpeechService _textToSpeech;
		readonly IMvxMessenger _messenger;
		readonly IBatteryDrainService _batteryDrainService;
		readonly ISpeechToTextService _speechToTextService;

		MvxSubscriptionToken _token;

		object lockOebject = new object ();

		#endregion

		#region Constructor

		public TrackFacade ()
		{
			_textToSpeech = Mvx.Resolve<ITextToSpeechService> ();
			_messenger = Mvx.Resolve<IMvxMessenger> ();
			_geoWatcher = Mvx.Resolve<IGeoLocationWatcher> ();
			_activity = Mvx.Resolve<IMotionActivity> ();
			_batteryDrainService = Mvx.Resolve<IBatteryDrainService>();
			_speechToTextService = Mvx.Resolve<ISpeechToTextService>();
		}

		#endregion

		#region Properties

		private bool _isBound;

		#endregion

		public virtual async void StartServices ()
		{			
			bool isGranted = await Mvx.Resolve<IPermissionsService> ().CheckPermissionsAccesGrantedAsync ();
			if (!_isBound & isGranted) {

				Log.LogMessage (string.Format ("THE SEVICES HAS STARTED AT {0}", DateTime.Now));

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

				Log.LogMessage (string.Format ("THE SEVICES HAS STOPPED AT {0}", DateTime.Now));

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

				Log.LogMessage (TollStatus.ToString ());
				TollStatus = statusObject.CheckStatus ();

				switch(TollStatus)
				{
					case TollGeolocationStatus.NotOnTollRoad:
						_batteryDrainService.CheckGpsTrackingSleepTime();
						break;
					case TollGeolocationStatus.NearTollRoadEnterce:
						_speechToTextService.AskQuestion("Are you entering the tollroad?");
						break;
					case TollGeolocationStatus.NearTollRoadExit:
						_speechToTextService.AskQuestion("Are you exiting from the tollroad?");
						break;
				}

				_geoWatcher.StartGeolocationWatcher();
			}
		}
	}
}