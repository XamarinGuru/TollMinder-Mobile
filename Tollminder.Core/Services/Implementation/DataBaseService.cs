using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Plugins.Sqlite;
using SQLiteNetExtensions.Extensions;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;
using Xamarin.Forms;

namespace Tollminder.Core.Services.Implementation
{
    public class DataBaseService : IDataBaseService
    {
        readonly ICheckerAppFirstLaunch _checkerAppFirstLaunch;
        private User _user;
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
            //_checkerAppFirstLaunch = Mvx.Resolve<ICheckerAppFirstLaunch>();
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
            Connection.CreateTable<User>();
            Connection.CreateTable<TollPoint>();
            Connection.CreateTable<TollRoadWaypoint>();
            Connection.CreateTable<TollRoad>();
        }

        public void InsertOrUpdateAllTollRoads(IList<TollRoad> tollRoads)
        {
            try
            {
                //if (!_checkerAppFirstLaunch.IsAppAlreadyLaunchedOnce())
                //{
                //    Connection.DeleteAll<TollRoad>();
                //    Connection.DeleteAll<TollPoint>();
                //    Log.LogMessage("App launched for the first time!!!!!");
                //}
                Log.LogMessage("App launched not for the first time!!!!!");
                //DeleteOldTollRoads(tollRoads);
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
                var tollRoadsToRemove = GetPointsToRemove(tollRoads, tollRoadsFromLocalDataBase);//Connection.GetAllWithChildren<TollRoad>(x => roadIds.Contains(x.Id), true);
                if (tollRoadsToRemove != null)
                {
                    Output(tollRoadsToRemove);
                }
                Connection.DeleteAll(tollRoadsToRemove, true);
                var tollR = Connection.GetAllWithChildren<TollRoad>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + " " + ex.StackTrace);
            }
        }

        public void SetUser(User user)
        {
            Connection.InsertOrReplace(user);
        }

        async Task<User> GetUser(string token)
        {
            var userFromDataBase = Connection.Get<User>(x => x.Token == token);
            var _serverApiService = Mvx.Resolve<IServerApiService>();
            _user = await _serverApiService.GetUser(userFromDataBase.Id);

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
            var elemWp = new List<TollRoadWaypoint>();
            var elemTp = new List<TollPoint>();
            var roadsIds = tollRoads.Select(x => x.Id);

            //foreach(var tollRoad in tollRoadsFromLocalDatabase)
            //{
            //    //GetElementsToRemove(tollRoad);
            //    if (!roadsIds.Contains(tollRoad.Id))
            //        elementsToRemove.Add(tollRoad);
            //    else
            //    {
            //        foreach (var wayPoint in tollRoad.WayPoints)
            //        {
            //            if (!tollRoads.Select(x => x.WayPoints).Contains(tollRoad.WayPoints))
            //                elemWp.Add(wayPoint);
            //            else {
            //                foreach (var tollPoint in wayPoint.TollPoints)
            //                {
            //                    if (tollPoint.Id != tollRoad.Id)
            //                        elemTp.Add(tollPoint);
            //                }
            //            }
            //        }
            //        elementsToRemove.Add(new TollRoad{
            //            Id = tollRoad.Id,
            //            Name = tollRoad.Name,
            //            Latitude = tollRoad.Latitude,
            //            Longitude = tollRoad.Longitude,
            //            WayPoints = 
            //        };
            //    }
            //        //GetElementsToRemove(tollRoad.WayPoints.FindAll(tollRoads.Select(wp => wp.Id)));
                    
            //}
            return elementsToRemove;
        }

        List<TollRoad> GetElementsToRemove(object tollRoad)
        {
            tollRoad.GetType().GetProperties();
            List<TollRoad> elementsToRemove = new List<TollRoad>();
            foreach (var road in tollRoad.GetType().GetProperties())
            {
                Debug.WriteLine(road.Name);
                //if (tollRoad.)
                //{
                //    GetElementsToRemove();
                //}
                //elementsToRemove.Add(tollRoad);
            }
            return elementsToRemove;
        }
    }
}
