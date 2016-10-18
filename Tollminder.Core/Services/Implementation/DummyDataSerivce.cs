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
            this._distanceChecker = distanceChecker;
            _dummyTollRoads = new List<TollRoad>()
            {
                new TollRoad()
                {
                    Id = 0,
                    Name = "POH tollroad east",
                    WayPoints = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Osokorki enterce",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.396344,
                                        Longitude = 30.620942
                                    },
                                    WaypointAction = WaypointAction.Enterce
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pozniaki near Billa exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.397892,
                                        Longitude = 30.631900
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Kharkivska enterce",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.401921,
                                        Longitude = 30.657414
                                    },
                                    WaypointAction = WaypointAction.Enterce
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Virlitsa exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.402902,
                                        Longitude = 30.670538
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Borispilska exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.402758,
                                        Longitude = 30.680092
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Kharkivska bridge",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.401120,
                                        Longitude = 30.652072
                                    },
                                    WaypointAction = WaypointAction.EntranceAndExit
                                }
                            }
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 1,
                    Name = "POH tollroad west",
                    WayPoints = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Pozniaki near Lake enterce",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.400262,
                                        Longitude = 30.645412
                                    },
                                    WaypointAction = WaypointAction.Enterce
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pozniaki near TNK  exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude =  50.398933,
                                        Longitude = 30.636302
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pozniaki near KLO exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.398068,
                                        Longitude = 30.629106
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Osokorki exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.393487,
                                        Longitude = 30.616838
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Osokorki bridge",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 50.396300,
                                        Longitude = 30.615771
                                    },
                                    WaypointAction = WaypointAction.EntranceAndExit
                                }
                            }
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 5,
                    Name = "15s",
                    WayPoints = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Temecula entrance",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.479557,
                                        Longitude = -117.140821
                                    },
                                    WaypointAction = WaypointAction.Enterce
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Rainbow Valley Exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.431537,
                                        Longitude = -117.146470
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Rainbow Valley Entrance",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.429156,
                                        Longitude = -117.147947
                                    },
                                    WaypointAction = WaypointAction.Enterce
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Old 365 Exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.388171,
                                        Longitude = -117.175638
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        }
                    }
                },

                new TollRoad()
                {
                    Id = 6,
                    Name = "15n",
                    WayPoints = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Old 365 Entrance",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.389516,
                                        Longitude =  -117.174028
                                    },
                                    WaypointAction = WaypointAction.Enterce
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Rainbow Valley Exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.429848,
                                        Longitude = -117.144840
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Rainbow Valley Entrance",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.431869,
                                        Longitude = -117.143373
                                    },
                                    WaypointAction = WaypointAction.Enterce
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Temecula Exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.479535,
                                        Longitude = -117.139537
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 9,
                    Name = "Pechanga",
                    WayPoints = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Pechanga entrance",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.473324,
                                        Longitude = -117.12711
                                    },
                                    WaypointAction = WaypointAction.Enterce
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Loma Linda Exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.466661,
                                        Longitude = -117.118350
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Via Eduardo Exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.459104,
                                        Longitude = -117.108260
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pechanga Exit",
                            TollPoints = new List<TollPoint>()
                            {
                                new TollPoint()
                                {
                                    Location = new GeoLocation
                                    {
                                        Latitude = 33.455975,
                                        Longitude = -117.103091
                                    },
                                    WaypointAction = WaypointAction.Exit
                                }
                            }
                        }
                    }
                }
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
                z.Name = y.Name;
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

        public TollPoint FindNearestEntranceTollPoint(GeoLocation center, TollPoint ignoredWaypoint = null)
        {
            var points = GetAllEntranceTollPoints();

            if (ignoredWaypoint != null)
                points = points.Except(new List<TollPoint>() { ignoredWaypoint }).ToList();

            return _distanceChecker.GetLocationFromRadius(center, points);
        }

        public TollPoint FindNearestExitTollPoint(GeoLocation center, TollPoint ignoredWaypoint = null)
        {
            var points = GetAllExitTollPoints();

            if (ignoredWaypoint != null)
                points = points.Except(new List<TollPoint>() { ignoredWaypoint }).ToList();

            return _distanceChecker.GetLocationFromRadius(center, points);
        }

        public IList<TollPoint> GetAllEntranceTollPoints()
        {
            return _dummyTollPoints.Where(x => x.WaypointAction == WaypointAction.Enterce || x.WaypointAction == WaypointAction.EntranceAndExit).ToList();
        }

        public IList<TollPoint> GetAllExitTollPoints(long tollRoadId = -1)
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