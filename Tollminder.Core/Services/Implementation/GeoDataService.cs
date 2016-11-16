using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
    public class GeoDataService : IGeoDataService
    {
        readonly IDistanceChecker _distanceChecker;
        readonly IDataBaseService _dataBaseStorage;
        readonly IServerApiService _serverApiService;
        readonly IStoredSettingsService _storedSettingsService;

        public GeoDataService()
        {
            _distanceChecker = Mvx.Resolve<IDistanceChecker>();
            _dataBaseStorage = Mvx.Resolve<IDataBaseService>();
            _serverApiService = Mvx.Resolve<IServerApiService>();
            _storedSettingsService = Mvx.Resolve<IStoredSettingsService>();
        }

        public List<TollPointWithDistance> FindNearestEntranceTollPoints(GeoLocation center)
        {
            return _distanceChecker.GetMostClosestTollPoint(center, _dataBaseStorage.GetAllEntranceTollPoints(), SettingsService.WaypointLargeRadius);
        }

        public List<TollPointWithDistance> FindNearestExitTollPoints(GeoLocation center)
        {
            return _distanceChecker.GetMostClosestTollPoint(center, _dataBaseStorage.GetAllExitTollPoints(), SettingsService.WaypointLargeRadius);
        }

        public async Task RefreshTollRoads(CancellationToken token)
        {
            var currentTime = DateTime.UtcNow;

            var shouldUpdateTollRoads = currentTime - _storedSettingsService.LastSyncDateTime <= TimeSpan.FromDays(1);

            if (shouldUpdateTollRoads)
            {
                _storedSettingsService.LastSyncDateTime = currentTime;    
                var list = await _serverApiService.RefreshTollRoads(_storedSettingsService.LastSyncDateTime.UnixTime(), token);
                _dataBaseStorage.InsertOrUpdateAllTollRoads(list);
            }
        }

        public TollRoad GetTollRoad(long id)
        {
            return _dataBaseStorage.GetTollRoad(id);
        }

        public TollRoadWaypoint GetTollWayPoint(long id)
        {
            return _dataBaseStorage.GetTollWayPoint(id);
        }

        public TollPoint GetTollPoint(long id)
        {
            return _dataBaseStorage.GetTollPoint(id);
        }

        public IList<TollPoint> GetAllEntranceTollPoints()
        {
            return _dataBaseStorage.GetAllEntranceTollPoints();
        }

        public IList<TollPoint> GetAllExitTollPoints(long tollRoadId = -1)
        {
            return _dataBaseStorage.GetAllExitTollPoints();
        }
    }
}