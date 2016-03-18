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
		private readonly IList<MvxSubscriptionToken> _tokens;

		#endregion

		#region Constructor

		public TrackFacade ()
		{
			this._textToSpeech = Mvx.Resolve<ITextToSpeechService> ();
			this._messenger = Mvx.Resolve<IMvxMessenger> ();
			this._tokens = new List<MvxSubscriptionToken> ();
			this._geoWatcher = Mvx.Resolve<IGeoLocationWatcher> ();
			this._activity = Mvx.Resolve<IMotionActivity> ();
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
				_activity.StartDetection ();
				//_tokens.Add (_messenger.Subscribe<LocationMessage> (x => CheckTrackStatus ()));
				//_isBound = true;
			}	

		}

		public virtual void StopServices ()
		{	
			if (_isBound) {
				#if DEBUG 
				Log.LogMessage (string.Format ("THE SEVICES HAS STOPPED AT {0}", DateTime.Now));
				#endif
				_geoWatcher.StopGeolocationWatcher ();
				_activity.StopDetection ();
				DestoyTokens ();
				_isBound = false;
			}
		}

		public TollGeolocationStatus TollStatus { get; set; } = TollGeolocationStatus.NotOnTollRoad;

		protected virtual void CheckTrackStatus ()
		{
			BaseStatus statusObject = StatusesFactory.GetStatus (TollStatus);
			TollStatus = statusObject.CheckStatus ();
		}

		#region Helpers

		protected virtual void DestoyTokens ()
		{
			foreach (var item in _tokens) {
				item.Dispose ();
			}
		}

		#endregion
	}
}