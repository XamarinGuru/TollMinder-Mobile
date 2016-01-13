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

namespace Tollminder.Core.ServicesHelpers.Implementation 
{
	public class TrackFacade : ITrackFacade
	{
		public const int WaypointDistanceRequired = 10;

		#region Services
		private readonly IGeoDataServiceAsync _geoData;
		private readonly IGeoLocationWatcher _geoWatcher;
		private readonly IMotionActivity _activity;
		private readonly ITextToSpeechService _textToSpeech;
		private readonly IMvxMessenger _messenger;
		private readonly INotificationSender _notficationSender;
		private readonly IList<MvxSubscriptionToken> _tokens;
		private readonly IPlatform _platform;
		#endregion

		#region Constructor
		public TrackFacade ()
		{
			this._textToSpeech = Mvx.Resolve<ITextToSpeechService> ();
			this._activity = Mvx.Resolve<IMotionActivity> ();
			this._geoWatcher = Mvx.Resolve<IGeoLocationWatcher> ();
			this._geoData = Mvx.Resolve<IGeoDataServiceAsync> ();
			this._messenger = Mvx.Resolve<IMvxMessenger> ();
			this._notficationSender = Mvx.Resolve<INotificationSender> ();
			this._platform = Mvx.Resolve<IPlatform> ();

			this._tokens = new List<MvxSubscriptionToken> ();
		}
		#endregion

		#region Properties
		public GeoLocation CarLocation { get; private set; }
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

		public TollGeolocationStatus TrackStatus { get; private set; } = TollGeolocationStatus.NotOnTollRoad;

		#endregion

		public virtual void StartServices () 
		{				
			_geoWatcher.StartGeolocationWatcher ();
			_activity.StartDetection ();
			_tokens.Add (_messenger.SubscribeOnMainThread<LocationMessage> (async x => {
				CarLocation = x.Data;
				await CheckTrackStatus();
			}));
			_tokens.Add (_messenger.SubscribeOnMainThread<MotionMessage> (x => MotionType = x.Data));
		}

		public virtual void StopServices () 
		{			
			_geoWatcher.StopGeolocationWatcher ();
			_activity.StopDetection ();
			DestoyTokens ();
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

		#region Status Methods
		protected virtual Task LookingForTollEnterce ()
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
						EnabledHighAccuracy();
						NotifyUser (string.Format ("you are potentially going to enter one of our {0} wayponts.", TollRoadWaypoint));
					}
				}
			});
		}

		protected virtual void NotifyUser (string data)
		{
			if (!_platform.IsAppInForeground) {
				_textToSpeech.Speak (data);
				_notficationSender.SendLocalNotification ("Toll Minder", data);
			}
		}

		protected virtual  Task LookingForTollExit ()
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
						EnabledHighAccuracy();
						NotifyUser (string.Format ("you are potentially going to exit one of our {0} wayponts.", TollRoadWaypoint));
					}
				}
			});
		}

		protected virtual  void CheckEnteredToTollRoad ()
		{
			if (IsCloserToWaypoint ()) {
				if (DistaceBetweenCarAndWaypoint < WaypointDistanceRequired) {
					TrackStatus = TollGeolocationStatus.OnTollRoad;
					EnabledHighAccuracy ();
				}
			} else {
				TrackStatus = TollGeolocationStatus.NotOnTollRoad;
				EnabledHighAccuracy ();
			}
		}

		protected virtual  void CheckExitFromTollRoad ()
		{
			if (IsCloserToWaypoint ()) {
				if (DistaceBetweenCarAndWaypoint < WaypointDistanceRequired) {
					TrackStatus = TollGeolocationStatus.NotOnTollRoad;
					EnabledHighAccuracy ();
				}
			} else {
				TrackStatus = TollGeolocationStatus.OnTollRoad;
				EnabledHighAccuracy ();
			}
		}
		#endregion

		#region Helpers
		protected virtual  void SpeakIfStartMoving (MotionType value)
		{
			if (value != MotionType && CheckIsMovingByTheCar (value)) {
				_textToSpeech.Speak ("You start moving on the car");
			}
		}

		protected virtual  bool IsCloserToWaypoint ()
		{
			return CheckIsMovingByTheCar (MotionType) && LocationChecker.DistanceBetweenGeoLocations (CarLocation, TollRoadWaypoint) < DistaceBetweenCarAndWaypoint;
		}

		protected virtual  void EnabledHighAccuracy ()
		{
			_geoWatcher.GeofenceEnabled = true;
			_geoWatcher.StopUpdatingHighAccuracyLocation ();
		}
		
		protected virtual  void DisabledHighAccuracy ()
		{
			_geoWatcher.GeofenceEnabled = true;
			_geoWatcher.StopUpdatingHighAccuracyLocation ();
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
		#endregion
	}
}