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
		public const double WaypointDistanceRequired = 0.01;

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

		GeoLocation carLocation;

		public GeoLocation CarLocation {
			get {
				return carLocation;
			}
			private set {
				carLocation = value;
				#if DEBUG 
				Log.LogMessage (string.Format ("UPDATE CAR LOCATION {0}", value));
				#endif
			}
		}

		public TollRoadWaypoint LastTollRoadWaypoint { get; private set; }

		double distaceBetweenCarAndWaypoint;
		public double DistaceBetweenCarAndWaypoint {
			get {
				return distaceBetweenCarAndWaypoint;
			}
			private set {
				#if DEBUG 
				Log.LogMessage (string.Format ("THE DISTANCE HAS BEEN CHANGED : {1} {0}",distaceBetweenCarAndWaypoint, value));
				#endif
				distaceBetweenCarAndWaypoint = value;
			}
		}

		MotionType _motionType;

		public MotionType MotionType {
			get { return _motionType; }
			private set {	
				_motionType = value;
			}
		}

		TollGeolocationStatus trackStatus = TollGeolocationStatus.NotOnTollRoad;

		public TollGeolocationStatus TrackStatus {
			get {
				return trackStatus;
			}
			private set {
				trackStatus = value;
				#if DEBUG 
				Log.LogMessage (string.Format ("THE TRACKSTATUS HAS BEEN CHANGED : {0}", value));
				#endif
			}
		}

		private bool _isBound;

		#endregion

		public virtual void StartServices ()
		{			
			if (!_isBound) {
				#if DEBUG 
				Log.LogMessage (string.Format ("THE SEVICES HAS STARTED AT {0}", DateTime.Now));
				#endif
				_textToSpeech.IsEnabled = true;
				_geoWatcher.StartGeolocationWatcher ();
				_activity.StartDetection ();
				_tokens.Add (_messenger.Subscribe<LocationMessage> (async x => {
					CarLocation = x.Data;
					await CheckTrackStatus ().ConfigureAwait(false);
				}));
				_tokens.Add (_messenger.Subscribe<MotionMessage> (x => { 
					MotionType = x.Data;					
				}));
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
				_activity.StopDetection ();
				DestoyTokens ();
				_isBound = false;
			}
		}



		protected virtual async Task CheckTrackStatus ()
		{
			#if DEBUG 
			Log.LogMessage (string.Format ("TRY TO CHECK STATUS : {0}", TrackStatus));
			#endif
			switch (TrackStatus) {
			case TollGeolocationStatus.NotOnTollRoad:
					#if DEBUG 
				Log.LogMessage (string.Format ("LookingForTollEnterce"));
					#endif
				await LookingForTollEnterce ().ConfigureAwait (false);
				break;
			case TollGeolocationStatus.OnTollRoad:
					#if DEBUG 
				Log.LogMessage (string.Format ("LookingForTollExit"));
					#endif
				await LookingForTollExit ().ConfigureAwait (false);
				;
				break;
			case TollGeolocationStatus.NearTollRoadEnterce:
					#if DEBUG 
				Log.LogMessage (string.Format ("NearTollRoadEnterce"));
					#endif
				CheckEnteredToTollRoad ();
				break;
			case TollGeolocationStatus.NearTollRoadExit:
					#if DEBUG 
				Log.LogMessage (string.Format ("NearTollRoadExit"));
					#endif
				CheckExitFromTollRoad ();
				break;
			}
		}

		#region Status Methods

		protected virtual async Task LookingForTollEnterce ()
		{			
			#if DEBUG 
			Log.LogMessage (string.Format ("CHECKING IS MOVING ON THE CAR {0}", CheckIsMovingByTheCar (MotionType)));
			#endif
			if (CheckIsMovingByTheCar (MotionType)) {
				#if DEBUG 
				Log.LogMessage (string.Format ("TRY TO FIND WAYPOINT ENTERCE FROM 200 m"));
				#endif
				var waypoint = await CheckNearLocationForTollRoadAsync (CarLocation, WaypointAction.Enterce).ConfigureAwait(false);
				#if DEBUG 
				Log.LogMessage (string.Format ("CAR LOCATION {0} , WAYPOINT LOCATION {1}", CarLocation, waypoint));
				#endif
				if (waypoint == null || waypoint == LastTollRoadWaypoint)
					return;
				#if DEBUG 
				Log.LogMessage (string.Format ("FOUNDED WAYPOINT ENTERCE : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));
				#endif
				LastTollRoadWaypoint = waypoint;						
				TrackStatus = TollGeolocationStatus.NearTollRoadEnterce;
				DistaceBetweenCarAndWaypoint = LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location);
				#if DEBUG 
				Log.LogMessage (string.Format ("DISTANCE CAR TO WAYPOINT : {0}", DistaceBetweenCarAndWaypoint));
				#endif
				EnabledHighAccuracy ();
				#if DEBUG 
				Log.LogMessage (string.Format ("ENABLED HIGH ACCURACY"));
				#endif
				NotifyUser (string.Format ("you are potentially going to enter {0} waypoints.", LastTollRoadWaypoint.Name));						
			}		
		}

		protected virtual void NotifyUser (string data)
		{
			if (!_platform.IsAppInForeground) {				
				_notficationSender.SendLocalNotification ("Toll Minder", data);
			}
			_textToSpeech.Speak (data);
		}

		protected virtual async Task LookingForTollExit ()
		{	
			#if DEBUG 
			Log.LogMessage (string.Format ("CHECKING IS MOVING ON THE CAR {0}", CheckIsMovingByTheCar (MotionType)));
			#endif
			if (CheckIsMovingByTheCar (MotionType)) {
				#if DEBUG 
				Log.LogMessage (string.Format ("TRY TO FIND WAYPOINT EXIT FROM 200 m"));
				#endif
				var waypoint = await CheckNearLocationForTollRoadAsync (CarLocation, WaypointAction.Exit).ConfigureAwait (false);

				if (waypoint == null || waypoint == LastTollRoadWaypoint)
					return;
				#if DEBUG 
				Log.LogMessage (string.Format ("FOUNDED WAYPOINT EXIT : {0} AND WAYPOINT ACTION {1}", waypoint.Name, waypoint.WaypointAction));
				#endif
				LastTollRoadWaypoint = waypoint;

				TrackStatus = TollGeolocationStatus.NearTollRoadExit;
				DistaceBetweenCarAndWaypoint = LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location);
				#if DEBUG 
				Log.LogMessage (string.Format ("DISTANCE CAR TO WAYPOINT : {0}", DistaceBetweenCarAndWaypoint));
				#endif
				EnabledHighAccuracy ();
				#if DEBUG 
				Log.LogMessage (string.Format ("ENABLED HIGH ACCURACY"));
				#endif
				NotifyUser (string.Format ("you are potentially going to exit {0} waypoints.", LastTollRoadWaypoint.Name));
			}
		}

		protected virtual void CheckEnteredToTollRoad ()
		{
			#if DEBUG 
			Log.LogMessage (string.Format ("Is Closer to waypoint : {0}", IsCloserToWaypoint ()));
			#endif
			if (IsCloserToWaypoint ()) {
				#if DEBUG 
				Log.LogMessage (string.Format ("DISTANCE CAR TO WAYPOINT IS CLOSER"));
				#endif
				DistaceBetweenCarAndWaypoint = LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location);
				if (IsAtWaypoint ()) {
					#if DEBUG 
					Log.LogMessage (string.Format ("CROSS WAYPOINT BY THE CAR"));
					#endif
					TrackStatus = TollGeolocationStatus.OnTollRoad;
					NotifyUser (string.Format ("You are entered to {0}", LastTollRoadWaypoint.Name));
					#if DEBUG 
					Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));
					#endif
					DisabledHighAccuracy ();
				}
			} else {
				#if DEBUG 
				Log.LogMessage (string.Format ("AVOID THIS WAYPOINT"));
				#endif
				TrackStatus = TollGeolocationStatus.NotOnTollRoad;
				#if DEBUG 
				Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));
				#endif
				DisabledHighAccuracy ();
			}
		}

		protected virtual void CheckExitFromTollRoad ()
		{
			#if DEBUG 
			Log.LogMessage (string.Format ("Is Closer to exit : {0}", IsCloserToWaypoint ()));
			#endif
			if (IsCloserToWaypoint ()) {
				#if DEBUG 
				Log.LogMessage (string.Format ("DISTANCE CAR TO WAYPOINT EXIT IS CLOSER"));
				#endif
				DistaceBetweenCarAndWaypoint = LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location);
				if (IsAtWaypoint ()) {
					#if DEBUG 
					Log.LogMessage (string.Format ("CROSS WAYPOINT BY THE CAR"));
					#endif
					TrackStatus = TollGeolocationStatus.NotOnTollRoad;
					NotifyUser (string.Format ("You are exit from {0}", LastTollRoadWaypoint.Name));
					#if DEBUG 
					Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));
					#endif
					DisabledHighAccuracy ();
				}
			} else {
				#if DEBUG 
				Log.LogMessage (string.Format ("AVOID THIS WAYPOINT EXIT"));
				#endif
				TrackStatus = TollGeolocationStatus.OnTollRoad;
				#if DEBUG 
				Log.LogMessage (string.Format ("DISABLED HIGH ACCURACY"));
				#endif
				DisabledHighAccuracy ();
			}
		}


		#endregion

		#region Helpers

		private bool IsAtWaypoint ()
		{
			#if DEBUG 
			Log.LogMessage (string.Format ("DIS : {0}, DIST 2 {1} = {2}",DistaceBetweenCarAndWaypoint, WaypointDistanceRequired , DistaceBetweenCarAndWaypoint - WaypointDistanceRequired));
			#endif			
			return (DistaceBetweenCarAndWaypoint - WaypointDistanceRequired) < 0;
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
			#if DEBUG 
			Log.LogMessage (string.Format ("DISTANCE {0} {1}", LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location), DistaceBetweenCarAndWaypoint));
			#endif
			return DistaceBetweenCarAndWaypoint - LocationChecker.DistanceBetweenGeoLocations (CarLocation, LastTollRoadWaypoint.Location) >= 0;
		}

		protected virtual void EnabledHighAccuracy ()
		{
			_geoWatcher.GeofenceEnabled = false;
			_geoWatcher.StartUpdatingHighAccuracyLocation ();
		}

		protected virtual void DisabledHighAccuracy ()
		{
			_geoWatcher.GeofenceEnabled = true;
			_geoWatcher.StopUpdatingHighAccuracyLocation ();
		}

		protected virtual void DestoyTokens ()
		{
			foreach (var item in _tokens) {
				item.Dispose ();
			}
		}

		protected virtual Task<TollRoadWaypoint> CheckNearLocationForTollRoadAsync (GeoLocation location)
		{
			return _geoData.FindNearGeoLocationAsync (location);
		}

		protected virtual Task<TollRoadWaypoint> CheckNearLocationForTollRoadAsync (GeoLocation location, WaypointAction actionStatus)
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