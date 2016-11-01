using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;
using MvvmCross.Plugins.Sqlite;
using SQLiteNetExtensions.Extensions;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
    public class DataBaseService : IDataBaseService
    {
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
        }

        public TollRoad GetTollRoad(long id)
        {
            var road = Connection.Get<TollRoad>(id);

            if (road == null)
                throw new Exception($"TollRoad with id = {id} was not found");

            return road;
        }

        public TollRoadWaypoint GetTollWayPoint(long id)
        {
            var wayPoint = Connection.Get<TollRoadWaypoint>(id);

            if (wayPoint == null)
                throw new Exception($"WayPoint with id = {id} was not found");

            return wayPoint;
        }

        public TollPoint GetTollPoint(long id)
        {
            var tollPoint = Connection.Get<TollPoint>(id);

            if (tollPoint == null)
                throw new Exception($"TollPoint with id = {id} was not found");

            return tollPoint;
        }

        void TryCreateTables()
        {
            Connection.CreateTable<GeoLocation>();
            Connection.CreateTable<TollPoint>();
            Connection.CreateTable<TollRoadWaypoint>();
            Connection.CreateTable<TollRoad>();
        }

        public void InsertOrUpdateAllTollRoads(IList<TollRoad> tollRoads)
        {
            DeleteOldTollRoads(tollRoads);
            Connection.InsertOrReplaceAllWithChildren(tollRoads, true);
        }

        public IList<TollPoint> GetAllEntranceTollPoints()
        {
            var list = Connection.GetAllWithChildren<TollPoint>(x => x.WaypointAction == WaypointAction.Entrance || x.WaypointAction == WaypointAction.Bridge, true).ToList();
            return list;
        }

        public IList<TollPoint> GetAllExitTollPoints(long tollRoadId = -1)
        {
            if (tollRoadId == -1)
                return Connection.GetAllWithChildren<TollPoint>(x => x.WaypointAction == WaypointAction.Exit);
            else
            {
                var road = Connection.GetWithChildren<TollRoad>(tollRoadId);
                var waypointIds = road.WayPoints.Select(x => x.Id).ToList();
                return Connection.GetAllWithChildren<TollPoint>(x => x.WaypointAction == WaypointAction.Exit && waypointIds.Contains(x.TollWaypointId));
            }
        }

        void DeleteOldTollRoads(IList<TollRoad> tollRoads)
        {
            var roadIds = tollRoads.Select(x => x.Id);
            var tollRoadsToRemove = Connection.GetAllWithChildren<TollRoad>(x => roadIds.Contains(x.Id), true);
            Connection.DeleteAll(tollRoadsToRemove, true);
        }
    }
}
