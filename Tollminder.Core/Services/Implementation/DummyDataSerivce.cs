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
        List<TollRoadWaypoint> _dummyWaypoints;
        IDistanceChecker _distanceChecker;

        public DummyDataSerivce(IDistanceChecker distanceChecker)
        {
            this._distanceChecker = distanceChecker;
            _dummyTollRoads = new List<TollRoad>()
            {
                new TollRoad()
                {
                    Id = 0,
                    Name = "POH tollroad",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Osokorki enterce",
                            Location = new GeoLocation 
                            {
                                Latitude = 50.396344, 
                                Longitude = 30.620942
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint 
                        {
                            Name = "Osokorki exit",
                            Location = new GeoLocation 
                            {
                                Latitude = 50.393487,
                                Longitude = 30.616838
                             },
                             WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pozniaki near Lake enterce",
                            Location = new GeoLocation
                            {
                                Latitude = 50.400262, 
                                Longitude = 30.645412
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pozniaki near Billa exit",
                            Location = new GeoLocation
                            {
                                Latitude = 50.397892, 
                                Longitude = 30.631900
                             },
                             WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pozniaki near TNK  exit",
                            Location = new GeoLocation
                            {
                                Latitude =  50.398933, 
                                Longitude = 30.636302
                             },
                             WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pozniaki near KLO exit",
                            Location = new GeoLocation
                            {
                                Latitude = 50.398068, 
                                Longitude = 30.629106
                             },
                             WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Kharkivska enterce",
                            Location = new GeoLocation
                            {
                                Latitude = 50.401921, 
                                Longitude = 30.657414
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Virlitsa exit",
                            Location = new GeoLocation
                            {
                                Latitude = 50.402902, 
                                Longitude = 30.670538
                             },
                             WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Borispilska exit",
                            Location = new GeoLocation
                            {
                                Latitude = 50.402758,
                                Longitude = 30.680092
                             },
                             WaypointAction = WaypointAction.Exit
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 5,
                    Name = "15s",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Temecula entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.479557,
                                Longitude = -117.140821
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Rainbow Valley Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.431537, 
                                Longitude = -117.146470
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Rainbow Valley Entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.429156, 
                                Longitude = -117.147947
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Old 365 Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.388171, 
                                Longitude = -117.175638
                            },
                            WaypointAction = WaypointAction.Exit
                        }
                    }
                },

                new TollRoad()
                {
                    Id = 6,
                    Name = "15n",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Old 365 Entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.389516,
                                Longitude =  -117.174028
                            },
                            WaypointAction = WaypointAction.Enterce
                        }
                        new TollRoadWaypoint
                        {
                            Name = "Rainbow Valley Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.429848, 
                                Longitude = -117.144840
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Rainbow Valley Entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.431869,
                                Longitude = -117.143373
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Temecula Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.479535, 
                                Longitude = -117.139537
                            },
                            WaypointAction = WaypointAction.Exit
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 9,
                    Name = "Pechanga",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Pechanga entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.473324,
                                Longitude = -117.12711
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Loma Linda Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.466661, 
                                Longitude = -117.118350
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Via Eduardo Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.459104, 
                                Longitude = -117.108260
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pechanga Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.455975,
                                Longitude = -117.103091
                            },
                            WaypointAction = WaypointAction.Exit
                        }
                    }
                }
            };
            int counter = 0;
            _dummyWaypoints = _dummyTollRoads.SelectMany(x => x.Points.Select(y => 
            {
                y.TollRoadId = x.Id;
                y.Id = ++counter;
                return y;
            })).ToList();
        }

        #region IGeoDataService implementation

        public TollRoad GetTollRoad(long id)
        {
            var road = _dummyTollRoads.FirstOrDefault(x => x.Id == id);

            if (road == null)
                throw new Exception($"TollRoad with id = {id} was not found");
            
            return road;
        }

        public TollRoadWaypoint FindNearestWaypoint(GeoLocation center, WaypointAction action, TollRoadWaypoint ignoredWaypoint = null)
        {
            return _distanceChecker.GetLocationFromRadius(center,(ignoredWaypoint != null) ? GetAllWaypoints(action).Where(x => x.Id != ignoredWaypoint.Id).ToList() : GetAllWaypoints(action));
        }

        public IList<TollRoadWaypoint> GetAllWaypoints(WaypointAction action, long tollRoadId = -1)
        {
            if (tollRoadId == -1)
                return _dummyWaypoints.Where(x => x.WaypointAction == action).ToList();
            else
                return _dummyWaypoints.Where(x => x.WaypointAction == action && x.TollRoadId == tollRoadId).ToList();
        }

        public TollRoadWaypoint FindNextExitWaypoint(GeoLocation center, TollRoadWaypoint currentWaypoint)
        {
            var points = _dummyTollRoads.FirstOrDefault(x => x.Id == currentWaypoint.TollRoadId);

            return _distanceChecker.GetLocationFromRadius(center, points?.Points.Where(x => x.Id != currentWaypoint.Id).ToList());
        }

        #endregion
    }
}