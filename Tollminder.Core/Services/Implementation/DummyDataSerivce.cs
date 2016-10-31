using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
    public class DummyDataSerivce : IGeoDataService
    {
        List<TollRoad> _dummyTollRoads;
        List<TollRoadWaypoint> _dummyWayPoints;
        List<TollPoint> _dummyTollPoints;
        IDistanceChecker _distanceChecker;

        public DummyDataSerivce(IDistanceChecker distanceChecker)
        {
            _distanceChecker = distanceChecker;
            _dummyTollRoads = new List<TollRoad>()
            {
                new TollRoad("POH tollroad east",new List<TollPoint>()
                {
                    new TollPoint("Osokorki enterce","50.396344, 30.620942","Entrance"),
                    new TollPoint("Pozniaki near Billa exit","50.397892, 30.631900","Exit"),
                    new TollPoint("Virlitsa exit","50.402902, 30.670538","Exit"),
                    new TollPoint("Borispilska exit","50.402758,  30.680092","Exit"),
                    new TollPoint("Kharkivska bridge","50.401120, 30.652072","Bridge")
                }),
                new TollRoad("POH tollroad west",new List<TollPoint>()
                {
                    new TollPoint("Pozniaki near Lake enterce","50.400262, 30.645412","Entrance"),
                    new TollPoint("Pozniaki near TNK  exit","50.398933, 30.636302","Exit"),
                    new TollPoint("Pozniaki near KLO exit","50.398068, 30.629106","Exit"),
                    new TollPoint("Osokorki exit","50.393487,  30.616838","Exit")
                }),
                new TollRoad("TollRoad s15",new List<TollPoint>()
                {
                    new TollPoint("Temecula entrance","33.479557, -117.140821","Entrance"),
                    new TollPoint("Rainbow Valley Exit","33.431537, -117.146470","Exit"),
                    new TollPoint("Rainbow Valley Entrance","33.429156, -117.147947","Entrance"),
                    new TollPoint("Old 365 Exit","33.388171, -117.175638","Exit")
                }),
                new TollRoad("TollRoad n15",new List<TollPoint>()
                {
                    new TollPoint("Temecula exit","33.479535, -117.139537","Exit"),
                    new TollPoint("Rainbow Valley Exit","33.429848, -117.144840","Exit"),
                    new TollPoint("Rainbow Valley Entrance","33.431869, -117.143373","Entrance"),
                    new TollPoint("Old 365 Entrance","33.389516, -117.174028","Entrance")
                }),
                new TollRoad("Pechanga",new List<TollPoint>()
                {
                    new TollPoint("Pechanga bridge","33.473411, -117.127120","Bridge"),
                    new TollPoint("Pechanga bridge 2","33.472443, -117.124787","Bridge")
                })
            };
            int waypointCounter = 0;
            _dummyWayPoints = _dummyTollRoads.SelectMany(x => x.WayPoints.Select(y =>
           {
               y.TollRoadId = x.Id;
               y.Id = ++waypointCounter;
               return y;
           })).ToList();
            int tollPointCounter = 0;
            _dummyTollPoints = _dummyTollRoads.SelectMany(x => x.WayPoints.SelectMany(y => y.TollPoints.Select(z =>
            {
                z.Id = ++tollPointCounter;
                z.TollWaypointId = y.Id;
                return z;
            }))).ToList();
        }

        #region IGeoDataService implementation

        public TollRoad GetTollRoad(long id)
        {
            var road = _dummyTollRoads.FirstOrDefault(x => x.Id == id);

            if (road == null)
                throw new Exception($"TollRoad with id = {id} was not found");

            return road;
        }

        public TollRoadWaypoint GetTollWayPoint(long id)
        {
            var wayPoint = _dummyWayPoints.FirstOrDefault(x => x.Id == id);

            if (wayPoint == null)
                throw new Exception($"WayPoint with id = {id} was not found");

            return wayPoint;
        }

        public TollPoint GetTollPoint(long id)
        {
            var tollPoint = _dummyTollPoints.FirstOrDefault(x => x.Id == id);

            if (tollPoint == null)
                throw new Exception($"TollPoint with id = {id} was not found");

            return tollPoint;
        }

        public List<TollPointWithDistance> FindNearestEntranceTollPoints(GeoLocation center)
        {
            return _distanceChecker.GetMostClosestTollPoint(center, GetAllEntranceTollPoints(), SettingsService.WaypointLargeRadius);
        }

        public List<TollPointWithDistance> FindNearestExitTollPoints(GeoLocation center)
        {
            return _distanceChecker.GetMostClosestTollPoint(center, GetAllExitTollPoints(), SettingsService.WaypointLargeRadius);
        } 

        public List<TollPoint> GetAllEntranceTollPoints()
        {
            return _dummyTollPoints.Where(x => x.WaypointAction == WaypointAction.Entrance || x.WaypointAction == WaypointAction.Bridge).ToList();
        }

        public List<TollPoint> GetAllExitTollPoints(long tollRoadId = -1)
        {
            var points = _dummyTollPoints.Where(x => x.WaypointAction == WaypointAction.Exit);

            if (tollRoadId == -1)
                return points.ToList();
            else
            {
                var waypoint = _dummyWayPoints.FirstOrDefault(x => x.TollRoadId == tollRoadId);
                return points.Where(x => x.TollWaypointId == waypoint?.Id).ToList();
            }
        }
        #endregion
    }
}