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

			TollStatus = TollGeolocationStatus.NotOnTollRoad;
			_isBound = Mvx.Resolve<IStoredSettingsService>().GeoWatcherIsRunning;
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

		TollGeolocationStatus _tollStatus;
		public TollGeolocationStatus TollStatus 
		{
			get
			{
				return _tollStatus;
			}
			set
			{
				_tollStatus = value;
				_messenger.Publish(new StatusMessage(this, value));
			}
		} 

		protected virtual async void CheckTrackStatus ()
		{
			BaseStatus statusObject = StatusesFactory.GetStatus (TollStatus);

			Log.LogMessage (TollStatus.ToString ());
			TollStatus = await statusObject.CheckStatus ();

			statusObject = StatusesFactory.GetStatus(TollStatus);

			Mvx.Resolve<INotificationSender>().SendLocalNotification($"Status: {TollStatus.ToString()}", $"Lat: {_geoWatcher.Location?.Latitude}, Long: {_geoWatcher.Location?.Longitude}");
		}
	}
}