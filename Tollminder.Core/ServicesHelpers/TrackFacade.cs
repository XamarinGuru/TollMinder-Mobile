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
using Tollminder.Core.Helpers;

namespace Tollminder.Core.ServicesHelpers
{
	public class TrackFacade 
	{
		public const int WaypointDistanceRequired = 10;

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

		GeoLocation _carLocation;
		public GeoLocation CarLocation {
			get { return _carLocation; }
			private set {
				_carLocation = value;
			}
		}
		public GeoLocation TollRoadWaypoint { get; private set; }
		public double DistaceBetweenCarAndWaypoint { get; private set; }

		MotionType _motionType;
		public MotionType MotionType {
			get { return _motionType; }
			private set {
				SpeakIfStartMoving (value);
				_motionType = value;

			}
		}

		TollGeolocationStatus _trackStatus = TollGeolocationStatus.NotOnTollRoad;
		public TollGeolocationStatus TrackStatus {
			get { return _trackStatus;	}
			private set {
				_trackStatus = value;
				switch (value) {
				case TollGeolocationStatus.NotOnTollRoad:
					break;
				case TollGeolocationStatus.NearTollRoadEnterce:
					break;
				case TollGeolocationStatus.NearTollRoadExit:
					break;
				case TollGeolocationStatus.OnTollRoad:
					break;
				default:
					throw new ArgumentOutOfRangeException ();
				}
			}
		}

		public void StartServices () 
		{				
			_geoWatcher.StartGeolocationWatcher ();
			_activity.StartDetection ();
			_tokens.Add (_messenger.SubscribeOnMainThread<LocationMessage> (async x => {
				CarLocation = x.Data;
				await CheckTrackStatus();
			}));
			_tokens.Add (_messenger.SubscribeOnMainThread<MotionMessage> (x => MotionType = x.Data));
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

		protected virtual Task<GeoLocation> CheckNearLocationForTollRoadAsync(GeoLocation location)
		{
			return _geoData.FindNearGeoLocationAsync (location);
		}

		protected virtual bool CheckIsMovingByTheCar (MotionType motionType)
		{				
			return MotionType.Automotive == motionType;
		}

		protected virtual async Task CheckTrackStatus()
		{
			switch (TrackStatus) {
			case TollGeolocationStatus.NotOnTollRoad:
				await LookingForTollEnterce ();
				break;
			case TollGeolocationStatus.OnTollRoad:
				await LookingForTollExit ();
				break;
			case TollGeolocationStatus.NearTollRoadEnterce:
				CheckEnteredToTollRoad ();
				break;
			case TollGeolocationStatus.NearTollRoadExit:
				CheckExitFromTollRoad ();
				break;
			}
		}

		void SpeakIfStartMoving (MotionType value)
		{
			if (value != MotionType && CheckIsMovingByTheCar (value)) {
				_textToSpeech.Speak ("You start moving on the car");
			}
		}

		private Task LookingForTollEnterce ()
		{
			return Task.Run(async () => {
				if (CheckIsMovingByTheCar (MotionType)) {
					var waypoint = await CheckNearLocationForTollRoadAsync (CarLocation);
					if (waypoint == TollRoadWaypoint)
						return;
					TollRoadWaypoint =  waypoint;
					if (TollRoadWaypoint != null) {
						TrackStatus = TollGeolocationStatus.NearTollRoadEnterce;
						DistaceBetweenCarAndWaypoint = LocationChecker.DistanceBetweenGeoLocations (CarLocation, TollRoadWaypoint);
						_textToSpeech.Speak (string.Format ("you are potentially going to enter one of our {0} wayponts.", TollRoadWaypoint));
					}
				}
			});
		}

		private Task LookingForTollExit ()
		{
			return Task.Run (async () => {
				if (CheckIsMovingByTheCar (MotionType)) {
					var waypoint = await CheckNearLocationForTollRoadAsync (CarLocation);
					if (waypoint == TollRoadWaypoint)
						return;
					TollRoadWaypoint = waypoint;
					if (TollRoadWaypoint != null) {
						TrackStatus = TollGeolocationStatus.NearTollRoadExit;
						DistaceBetweenCarAndWaypoint = LocationChecker.DistanceBetweenGeoLocations (CarLocation, TollRoadWaypoint);
						_textToSpeech.Speak (string.Format ("you are potentially going to exit one of our {0} wayponts.", TollRoadWaypoint));
					}
				}
			});
		}

		private void CheckEnteredToTollRoad ()
		{
			if (IsCloserToWaypoint ()) {
				if (DistaceBetweenCarAndWaypoint < WaypointDistanceRequired) {
					TrackStatus = TollGeolocationStatus.OnTollRoad;
				}
			} else {
				TrackStatus = TollGeolocationStatus.NotOnTollRoad;
			}
		}

		private void CheckExitFromTollRoad ()
		{
			if (IsCloserToWaypoint ()) {
				if (DistaceBetweenCarAndWaypoint < WaypointDistanceRequired) {
					TrackStatus = TollGeolocationStatus.NotOnTollRoad;
				}
			} else {
				TrackStatus = TollGeolocationStatus.OnTollRoad;
			}
		}

		private bool IsCloserToWaypoint ()
		{
			return CheckIsMovingByTheCar (MotionType) && LocationChecker.DistanceBetweenGeoLocations (CarLocation, TollRoadWaypoint) < DistaceBetweenCarAndWaypoint;
		}
	}
}

