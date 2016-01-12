using System;
using Tollminder.Core.Services;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Models;
using System.Threading.Tasks;
using System.Linq;
using Tollminder.Core.Utils;

namespace Tollminder.Core.ServicesHelpers
{
	public class TrackFacade 
	{
		#region Services
		private readonly IGeoDataServiceAsync _geoData;
		private readonly IGeoLocationWatcher _geoWatcher;
		private readonly IMotionActivity _activity;
		private readonly ITextToSpeechService _textToSpeech;
		private readonly IMvxMessenger _messenger;
		private readonly IList<MvxSubscriptionToken> _tokens;

		#endregion

		public TrackFacade ()
		{
			this._textToSpeech = Mvx.Resolve<ITextToSpeechService> ();
			this._activity = Mvx.Resolve<IMotionActivity> ();
			this._geoWatcher = Mvx.Resolve<IGeoLocationWatcher> ();
			this._geoData = Mvx.Resolve<IGeoDataServiceAsync> ();
			this._tokens = new List<MvxSubscriptionToken> ();
			this._messenger = Mvx.Resolve<IMvxMessenger> ();
		}

		public GeoLocation Location { get; private set; }
		MotionType _motionType;
		public MotionType MotionType {
			get { return _motionType; }
			private set {
				if (value != MotionType && CheckIsMovingByTheCar(value)) {
					_textToSpeech.Speak ("You start moving on the car");
				}
				_motionType = value;

			}
		}

		public void StartServices () 
		{				
			_geoWatcher.StartGeolocationWatcher ();
			_activity.StartDetection ();
			_tokens.Add (_messenger.SubscribeOnMainThread<LocationMessage> (async x => await CheckForTollRoadsAsync (x.Data)));
			_tokens.Add (_messenger.SubscribeOnMainThread<MotionMessage> (x => CheckIsMovingByTheCar(x.Data)));
		}

		public void StopServices () 
		{			
			_geoWatcher.StopGeolocationWatcher ();
			_activity.StopDetection ();
			DestoyTokens ();

		}

		protected virtual void DestoyTokens()
		{
			foreach (var item in _tokens) {
				item.Dispose ();
			}
		}

		protected virtual async Task CheckForTollRoadsAsync(GeoLocation loc)
		{
			Location = loc;
			if (CheckIsMovingByTheCar(MotionType)) {
				var result = await CheckNearLocationsForTollRoadAsync (Location);
				if (result?.Count() != 0) {
					foreach (var item in result) {
						_textToSpeech.Speak (string.Format ("you are potentially going to enter one of our {0} wayponts.", item));
					}
				}
			}
		}

		protected virtual Task<ParallelQuery<GeoLocation>> CheckNearLocationsForTollRoadAsync(GeoLocation location)
		{
			return _geoData.FindNearGeoLocationsAsync (location);
		}

		protected virtual bool CheckIsMovingByTheCar (MotionType motionType)
		{				
			return MotionType.Automotive == motionType;
		}
	}
}

