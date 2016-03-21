using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using System;
using Tollminder.Core.Models.Statuses;

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
		private readonly AutomoveActivity _automove;

		private MvxSubscriptionToken _token;

		#endregion

		#region Constructor

		public TrackFacade ()
		{
			this._textToSpeech = Mvx.Resolve<ITextToSpeechService> ();
			this._messenger = Mvx.Resolve<IMvxMessenger> ();
			this._geoWatcher = Mvx.Resolve<IGeoLocationWatcher> ();
			this._activity = Mvx.Resolve<IMotionActivity> ();
			_automove = new AutomoveActivity ();
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
				//_geoWatcher.StartGeolocationWatcher ();
				_activity.StartDetection ();
				_automove.Automoved += IsStartedMoveOnTheCar;
			
				_isBound = true;
			}	

		}


		public virtual void StopServices ()
		{	
			if (_isBound) {
				#if DEBUG 
				Log.LogMessage (string.Format ("THE SEVICES HAS STOPPED AT {0}", DateTime.Now));
				#endif
				//_geoWatcher.StopGeolocationWatcher ();
				_activity.StopDetection ();
				_automove.Automoved -= IsStartedMoveOnTheCar;
				_isBound = false;
			}
		}

		private void IsStartedMoveOnTheCar (object sender, bool e)
		{
			if (e) {
				_geoWatcher.StartGeolocationWatcher ();
				_token = _messenger.Subscribe<LocationMessage> (x => CheckTrackStatus ());
			} else {
				_token?.Dispose ();
				_geoWatcher.StopGeolocationWatcher ();
			}
		}

		public TollGeolocationStatus TollStatus { get; set; } = TollGeolocationStatus.NotOnTollRoad;

		protected virtual void CheckTrackStatus ()
		{
			BaseStatus statusObject = StatusesFactory.GetStatus (TollStatus);
			TollStatus = statusObject.CheckStatus ();
		}
	}
}