using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using System;

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
		public TollRoadWaypoint LastTollRoadWaypoint { get; private set; }
		public double DistaceBetweenCarAndWaypoint { get; private set; }

		MotionType _motionType;
		public MotionType MotionType {
			get { return _motionType; }
			private set {
				SpeakMotion (value);	
				_motionType = value;
			}
		}
		public TollGeolocationStatus TrackStatus { get; private set; } = TollGeolocationStatus.NotOnTollRoad;
		private bool _isBound;
		#endregion

		public virtual void StartServices () 
		{
			if (!_isBound) {				
				_textToSpeech.IsEnabled = true;
				_geoWatcher.StartGeolocationWatcher ();
				_activity.StartDetection ();
				_tokens.Add (_messenger.SubscribeOnMainThread<LocationMessage> (async x => {
					CarLocation = x.Data;
					await CheckTrackStatus();
				}));
				_tokens.Add (_messenger.SubscribeOnMainThread<MotionMessage> (x => { 
					MotionType = x.Data;					
				}));
				_isBound = true;
			}				
		}

		public virtual void StopServices () 
		{	
			if (_isBound) {
				_geoWatcher.StopGeolocationWatcher ();
				_activity.StopDetection ();
				DestoyTokens ();
				_isBound = false;
			}
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
					var waypoint = await CheckNearLocationForTollRoadAsync (CarLocation, WaypointAction.Enterce);
					if (waypoint == LastTollRoadWaypoint && waypoint == null)
						return;
					LastTollRoadWaypoint =  waypoint;
					if (LastTollRoadWaypoint != null) {
						TrackStatus = TollGeolocationStatus.NearTollRoadEnterce;
						DistaceBetweenCarAndWaypoint = Math.Round(LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location));
						EnabledHighAccuracy();
						NotifyUser (string.Format ("you are potentially going to enter {0} waypoints.", LastTollRoadWaypoint.Name));
					}
				}
			});
		}

		protected virtual void NotifyUser (string data)
		{
			if (!_platform.IsAppInForeground) {				
				_notficationSender.SendLocalNotification ("Toll Minder", data);
			}
			_textToSpeech.Speak (data);
		}

		protected virtual Task LookingForTollExit ()
		{
			return Task.Run (async () => {
				if (CheckIsMovingByTheCar (MotionType)) {
					var waypoint = await CheckNearLocationForTollRoadAsync (CarLocation, WaypointAction.Exit);
					if (waypoint == LastTollRoadWaypoint && waypoint == null)
						return;
					LastTollRoadWaypoint = waypoint;
					if (LastTollRoadWaypoint != null) {
						TrackStatus = TollGeolocationStatus.NearTollRoadExit;
						DistaceBetweenCarAndWaypoint = Math.Round(LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location));
						EnabledHighAccuracy();
						NotifyUser (string.Format ("you are potentially going to exit {0} waypoints.", LastTollRoadWaypoint.Name));
					}
				}
			});
		}

		protected virtual void CheckEnteredToTollRoad ()
		{
			#if DEBUG 
			Log.LogMessage(string.Format ("Is Closer to waypoint : {0}", IsCloserToWaypoint ()));
			#endif
			if (IsCloserToWaypoint ()) {
				if (IsAtWaypoint ()) {
					TrackStatus = TollGeolocationStatus.OnTollRoad;
					NotifyUser (string.Format ("You are entered to {0}", LastTollRoadWaypoint.Name));
					DisabledHighAccuracy ();
				}
			} else {
				TrackStatus = TollGeolocationStatus.NotOnTollRoad;
				DisabledHighAccuracy ();
			}
		}

		protected virtual void CheckExitFromTollRoad ()
		{
			#if DEBUG 
			Log.LogMessage(string.Format ("Is Closer to exit : {0}", IsCloserToWaypoint ()));
			#endif
			if (IsCloserToWaypoint ()) {
				if (IsAtWaypoint()) {
					TrackStatus = TollGeolocationStatus.NotOnTollRoad;
					NotifyUser (string.Format ("You are exit from {0}", LastTollRoadWaypoint.Name));
					DisabledHighAccuracy ();
				}
			} else {
				TrackStatus = TollGeolocationStatus.OnTollRoad;
				DisabledHighAccuracy ();
			}
		}


		#endregion

		#region Helpers

		private bool IsAtWaypoint ()
		{
			return Math.Abs (DistaceBetweenCarAndWaypoint - WaypointDistanceRequired) >= 0.0001;
		}

		protected virtual void SpeakMotion (MotionType value)
		{
			if (value != MotionType) {
				if (CheckIsMovingByTheCar (value)) {
					NotifyUser ("You start moving on the car");					
				} else {
					NotifyUser (value.ToString ());
				}
			}
		}

		protected virtual bool IsCloserToWaypoint ()
		{
			return Math.Abs(Math.Round(LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location)) - DistaceBetweenCarAndWaypoint) > 0.001;
		}

		protected virtual void EnabledHighAccuracy ()
		{
			_geoWatcher.GeofenceEnabled = true;
			_geoWatcher.StartUpdatingHighAccuracyLocation ();
		}
		
		protected virtual void DisabledHighAccuracy ()
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

		protected virtual Task<TollRoadWaypoint> CheckNearLocationForTollRoadAsync(GeoLocation location)
		{
			return _geoData.FindNearGeoLocationAsync (location);
		}

		protected virtual Task<TollRoadWaypoint> CheckNearLocationForTollRoadAsync(GeoLocation location, WaypointAction actionStatus)
		{
			return _geoData.FindNearGeoLocationAsync (location, actionStatus);
		}

		protected virtual bool CheckIsMovingByTheCar (MotionType motionType)
		{				
			return MotionType.Automotive == motionType;
		}
		#endregion
	}
}