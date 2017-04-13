using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Chance.MvvmCross.Plugins.UserInteraction;
using MvvmCross.Platform;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.RoadsProcessing;
using Tollminder.Core.Services.Settings;
using Xamarin;

namespace Tollminder.Core.Services.GeoData
{
    public class GeoDataService : IGeoDataService
    {
        readonly IDistanceChecker _distanceChecker;
        readonly IDataBaseService _dataBaseStorage;
        readonly IServerApiService _serverApiService;
        readonly IStoredSettingsService _storedSettingsService;

        public GeoDataService(IDistanceChecker distanceChecker, IDataBaseService dataBaseStorage, IServerApiService serverApiService, IStoredSettingsService storedSettingsService)
        {
            try
            {
                _distanceChecker = distanceChecker;
                _dataBaseStorage = dataBaseStorage;
                _serverApiService = serverApiService;
                _storedSettingsService = storedSettingsService;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Message:{0}\n StackTrace:{1}", ex.Message, ex.StackTrace);
            }
        }

        public List<TollPointWithDistance> FindNearestTollPoints(GeoLocation center)
        {
            return _distanceChecker.GetMostClosestTollPoint(center, GetAllTollPoints(), SettingsService.WaypointLargeRadius);
        }

        public List<TollPointWithDistance> FindNearestEntranceTollPoints(GeoLocation center)
        {
            return _distanceChecker.GetMostClosestTollPoint(center, _dataBaseStorage.GetAllEntranceTollPoints(), SettingsService.WaypointLargeRadius);
        }

        public List<TollPointWithDistance> FindNearestExitTollPoints(GeoLocation center)
        {
            return _distanceChecker.GetMostClosestTollPoint(center, _dataBaseStorage.GetAllExitTollPoints(), SettingsService.WaypointLargeRadius);
        }

        public async Task RefreshTollRoadsAsync(CancellationToken token)
        {
            try
            {
                var currentTime = DateTime.UtcNow;
                var timeSpan = TimeSpan.FromDays(1);
                //var shouldUpdateTollRoads = currentTime - _storedSettingsService.LastSyncDateTime > timeSpan;

                var list = await _serverApiService.RefreshTollRoadsAsync(_storedSettingsService.LastSyncDateTime.UnixTime(), token);
                if (list != null)
                {
                    _storedSettingsService.LastSyncDateTime = currentTime;
                    _dataBaseStorage.InsertOrUpdateAllTollRoads(list);
                }
                else
                {
                    Insights.Report(new NullReferenceException { Source = "Response, has no roads!" });
                    Mvx.Resolve<IUserInteraction>().Alert("App has not get any roads!", null, "Warning", "Ok");
                }
            }
            catch (Exception ex)
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

        public IList<TollPoint> GetAllTollPoints()
        {
            return _dataBaseStorage.GetAllTollPoints();
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