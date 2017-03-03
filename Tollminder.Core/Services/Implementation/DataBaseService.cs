using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Sqlite;
using SQLiteNetExtensions.Extensions;
using Tollminder.Core.Models;
using Xamarin;

namespace Tollminder.Core.Services.Implementation
{
    public class DataBaseService : IDataBaseService
    {
        readonly IStoredSettingsBase storedSettingsBase;
        private Profile _user;
        string databaseName = "tollminder.sqlite";
        SQLite.SQLiteConnection _connection;
        SQLite.SQLiteConnection Connection
        {
            get
            {
                return _connection ?? (_connection = Mvx.Resolve<IMvxSqliteConnectionFactory>().GetConnection(databaseName));
            }
        }

        public DataBaseService()
        {
            TryCreateTables();
            storedSettingsBase = Mvx.Resolve<IStoredSettingsBase>();
        }

        public TollRoad GetTollRoad(string id)
        {
            var road = Connection.Get<TollRoad>(id);

            if (road == null)
                throw new Exception($"TollRoad with id = {id} was not found");

            return road;
        }

        public TollRoadWaypoint GetTollWayPoint(string id)
        {
            var wayPoint = Connection.Get<TollRoadWaypoint>(id);

            if (wayPoint == null)
                throw new Exception($"WayPoint with id = {id} was not found");

            return wayPoint;
        }

        public TollPoint GetTollPoint(string id)
        {
            var tollPoint = Connection.Get<TollPoint>(id);

            if (tollPoint == null)
                throw new Exception($"TollPoint with id = {id} was not found");

            return tollPoint;
        }

        void TryCreateTables()
        {
            try
            {
                Connection.CreateTable<TollPoint>();
                Connection.CreateTable<TollRoadWaypoint>();
                Connection.CreateTable<TollRoad>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }

        public void InsertOrUpdateAllTollRoads(IList<TollRoad> tollRoads)
        {
            try
            {
                DeleteOldTollRoads(tollRoads);

                Connection.InsertOrReplaceAllWithChildren(tollRoads, true);
                var points = Connection.GetAllWithChildren<TollPoint>();
                var roads = Connection.GetAllWithChildren<TollRoad>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public IList<TollPoint> GetAllEntranceTollPoints()
        {
            var list = Connection.GetAllWithChildren<TollPoint>(x => x.WaypointAction == WaypointAction.Entrance || x.WaypointAction == WaypointAction.Bridge, true).ToList();
            return list;
        }

        public IList<TollPoint> GetAllExitTollPoints(string tollRoadId = "-1")
        {
            if (tollRoadId == "-1")
            {
                try
                {
                    var elem = Connection.GetAllWithChildren<TollPoint>(x => x.WaypointAction == WaypointAction.Exit);
                    return elem;
                }
                catch (Exception ex)
                {
                    Insights.Report(ex);
                    return null;
                }
            }
            else
            {
                var road = Connection.GetWithChildren<TollRoad>(tollRoadId);
                var waypointIds = road.WayPoints.Select(x => x.Id).ToList();
                return Connection.GetAllWithChildren<TollPoint>(x => x.WaypointAction == WaypointAction.Exit && waypointIds.Contains(x.TollWaypointId));
            }
        }

        void DeleteOldTollRoads(IList<TollRoad> tollRoads)
        {
            var tollRoadsFromLocalDataBase = Connection.GetAllWithChildren<TollRoad>(null, true);
            Output(tollRoadsFromLocalDataBase);
            try
            {
                var tollRoadsToRemove = GetPointsToRemove(tollRoads, tollRoadsFromLocalDataBase);
                if (tollRoadsToRemove != null)
                {
                    Output(tollRoadsToRemove);
                }
                Connection.DeleteAll(tollRoadsToRemove, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        public void SetUser(Profile user)
        {
            Connection.InsertOrReplace(user);
        }

        async Task<Profile> GetUser(string token)
        {
            var userFromDataBase = Connection.Get<Profile>(x => x.Token == token);
            var _serverApiService = Mvx.Resolve<IServerApiService>();
            _user = await _serverApiService.GetProfile(userFromDataBase.Id);

            return _user;
        }

        public async Task<bool> IsTokenStillValid(string token)
        {
            _user = await GetUser(token);
            return _user.Token != token ? false : true;
        }

        void Output(List<TollRoad> tollRoads)
        {
            foreach (var tollRoad in tollRoads)
            {
                Debug.WriteLine("Toolroad: Name - {0} \n \tId - {1}", tollRoad.Name, tollRoad.Id);

                foreach (var wayPoint in tollRoad.WayPoints)
                    Debug.WriteLine("{0} Waypoint: {1}", "\t", wayPoint.Name);
            }
        }

        List<TollRoad> GetPointsToRemove(IList<TollRoad> tollRoads, List<TollRoad> tollRoadsFromLocalDatabase)
        {
            List<TollRoad> elementsToRemove = new List<TollRoad>();

            return elementsToRemove;
        }
    }
}
