using System;
using System.Collections.Generic;
using MvvmCross.Platform;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
	public class StoredSettingsService : IStoredSettingsService
	{
		readonly IStoredSettingsBase _storedSettingsBase;

		public StoredSettingsService()
		{
            _storedSettingsBase = Mvx.Resolve<IStoredSettingsBase>();
		}

		public bool GeoWatcherIsRunning
		{
			get { return _storedSettingsBase.Get<bool>(); }
			set { _storedSettingsBase.Set<bool>(value); }
		}

		public GeoLocation Location
		{
			get { return _storedSettingsBase.Get<GeoLocation>(); }
			set { _storedSettingsBase.Set<GeoLocation>(value); }
		}

		public TollGeolocationStatus Status
		{
			get { return _storedSettingsBase.Get<TollGeolocationStatus>(); }
			set { _storedSettingsBase.Set<TollGeolocationStatus>(value); }
		}

        public List<TollPointWithDistance> TollPointsInRadius
		{
            get { return _storedSettingsBase.Get<List<TollPointWithDistance>>(); }
            set { _storedSettingsBase.Set<List<TollPointWithDistance>>(value); }
		}

        public TollRoad TollRoad
        {
            get { return _storedSettingsBase.Get<TollRoad>(); }
            set { _storedSettingsBase.Set<TollRoad>(value); }
        }

		public TollRoadWaypoint TollRoadEntranceWaypoint
		{
			get { return _storedSettingsBase.Get<TollRoadWaypoint>(); }
			set { _storedSettingsBase.Set<TollRoadWaypoint>(value); }
		}

        public DateTime TollRoadEntranceWaypointDateTime
        {
            get { return _storedSettingsBase.Get<DateTime>(); }
            set { _storedSettingsBase.Set<DateTime>(value); }
        }

		public TollRoadWaypoint TollRoadExitWaypoint
		{
			get { return _storedSettingsBase.Get<TollRoadWaypoint>(); }
			set { _storedSettingsBase.Set<TollRoadWaypoint>(value); }
		}

        public DateTime TollRoadExitWaypointDateTime
        {
            get { return _storedSettingsBase.Get<DateTime>(); }
            set { _storedSettingsBase.Set<DateTime>(value); }
        }

        public TollPoint IgnoredChoiceTollPoint
        {
            get { return _storedSettingsBase.Get<TollPoint>(); }
            set { _storedSettingsBase.Set<TollPoint>(value); }
        }

		public DateTime SleepGPSDateTime
		{
			get { return _storedSettingsBase.Get<DateTime>(); }
			set { _storedSettingsBase.Set<DateTime>(value); }
		}

        public bool IsAuthorized
        {
            get { return _storedSettingsBase.Get<bool>(); }
            set { _storedSettingsBase.Set<bool>(value); }
        }

        public DateTime LastSyncDateTime
        {
            get { return _storedSettingsBase.Get<DateTime>(new DateTime(1970, 1, 1)); }
            set { _storedSettingsBase.Set<DateTime>(value); }
        }
    }
}

