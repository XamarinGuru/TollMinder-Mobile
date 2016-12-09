using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            _checkerAppFirstLaunch = Mvx.Resolve<ICheckerAppFirstLaunch>();
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
            //Connection.CreateTable<GeoLocation>();
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
                    Connection.DeleteAll<TollRoad>();
                    Connection.DeleteAll<TollPoint>();
                    //Log.LogMessage("App launched for the first time!!!!!");
                //}
                Log.LogMessage("App launched not for the first time!!!!!");
                DeleteOldTollRoads(tollRoads);
                Connection.InsertOrReplaceAllWithChildren(tollRoads, true);
                var points = Connection.GetAllWithChildren<TollPoint>();
                var roads = Connection.GetAllWithChildren<TollRoad>();
            }
            catch(Exception e)
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
                catch(Exception ex){
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
            var tollRoad = Connection.GetAllWithChildren<TollRoad>(null, true);
            Output(tollRoad);
            var roadIds = tollRoads.Select(x => x.Id);
            try
            {
                var tollRoadsToRemove = GetElementsToRemove(roadIds, tollRoad);//Connection.GetAllWithChildren<TollRoad>(x => roadIds.Contains(x.Id), true);
                if (tollRoadsToRemove != null)
                {
                    Output(tollRoadsToRemove);
                }
                Connection.DeleteAll(tollRoadsToRemove, true);
                var tollR = Connection.GetAllWithChildren<TollRoad>();
            }
            catch(Exception ex){
                Debug.WriteLine(ex.Message +" "+ ex.StackTrace);
            }
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

        List<TollRoad> GetElementsToRemove(IEnumerable<string> roadsIds, List<TollRoad> tollRoads)
        {
            List<TollRoad> elementsToRemove = new List<TollRoad>();
            foreach(var tollRoad in tollRoads)
            {
                if (roadsIds.Contains(tollRoad.Id))
                    elementsToRemove.Add(tollRoad);
            }
            return elementsToRemove;
        }
    }
}
