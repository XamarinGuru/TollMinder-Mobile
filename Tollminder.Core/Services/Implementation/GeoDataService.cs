using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {
                _distanceChecker = Mvx.Resolve<IDistanceChecker>();
                _dataBaseStorage = Mvx.Resolve<IDataBaseService>();
                _serverApiService = Mvx.Resolve<IServerApiService>();
                _storedSettingsService = Mvx.Resolve<IStoredSettingsService>();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Message:{0}\n StackTrace:{1}", ex.Message, ex.StackTrace);
            }
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
            try
            {
                var currentTime = DateTime.UtcNow;
                var timeSpan = TimeSpan.FromDays(1);
                var shouldUpdateTollRoads = currentTime - _storedSettingsService.LastSyncDateTime > timeSpan;

                if (true)
                {
                    var list = await _serverApiService.RefreshTollRoads(_storedSettingsService.LastSyncDateTime.UnixTime(), token);
                    //Debug.WriteLine(list);
                    if (list != null)
                    {
                        _storedSettingsService.LastSyncDateTime = currentTime;
                        _dataBaseStorage.InsertOrUpdateAllTollRoads(list);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public TollRoad GetTollRoad(string id)
        {
            return _dataBaseStorage.GetTollRoad(id);
        }

        public TollRoadWaypoint GetTollWayPoint(string id)
        {
            return _dataBaseStorage.GetTollWayPoint(id);
        }

        public TollPoint GetTollPoint(string id)
        {
            return _dataBaseStorage.GetTollPoint(id);
        }

        public IList<TollPoint> GetAllEntranceTollPoints()
        {
            return _dataBaseStorage.GetAllEntranceTollPoints();
        }

        public IList<TollPoint> GetAllExitTollPoints(string tollRoadId = "-1")
        {
            return _dataBaseStorage.GetAllExitTollPoints();
        }
    }
}