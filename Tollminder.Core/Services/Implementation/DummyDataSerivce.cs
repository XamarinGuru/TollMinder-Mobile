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
                    Name = "Harkovkse metro",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Harkovkse metro enterce",
                            Location = new GeoLocation 
                            {
                                Latitude = 50.401130,
                                Longitude =  30.652835
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint 
                        {
                            Name = "Harkovkse metro exit",
                            Location = new GeoLocation 
                            {
                                Latitude = 50.403219,
                                Longitude =  30.667185,
                             },
                             WaypointAction = WaypointAction.Exit
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 1,
                    Name = "Kolektorna",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Kolektorna enterce",
                            Location = new GeoLocation
                            {
                                Latitude = 50.401042,
                                Longitude = 30.681115
                            },

                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Kolektorna exit",
                            Location = new GeoLocation
                            {
                                Latitude = 50.397919,
                                Longitude = 30.676943
                            },
                            WaypointAction = WaypointAction.Exit
                         }
                    }
                },
                new TollRoad()
                {
                    Id = 2,
                    Name = "Poznaki",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Poznaki Enterce",
                            Location = new GeoLocation
                            {
                                Latitude = 50.399954,
                                Longitude = 30.644416
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Poznaki Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 50.398490,
                                Longitude = 30.634127
                            },
                            WaypointAction = WaypointAction.Exit
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 3,
                    Name = "Osokorki",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Osokorki Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 50.395579,
                                Longitude = 30.616243
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Osokorki Enter",
                            Location = new GeoLocation
                            {
                                Latitude = 50.397062,
                                Longitude = 30.626107
                            },
                            WaypointAction = WaypointAction.Enterce
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 4,
                    Name = "Home",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Home Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 50.4014337,
                                Longitude = 30.5309801
                            },

                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Home enterce",
                            Location = new GeoLocation
                            {
                                Latitude = 50.431920,
                                Longitude = 30.515982
                            },
                            WaypointAction = WaypointAction.Enterce
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
                            Name = "15s entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.429156,
                                Longitude = -117.147946
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "15s Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.387116,
                                Longitude = -117.176185
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
                            Name = "15n entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.38717,
                                Longitude = -117.173374
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "15n Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.428762,
                                Longitude = -117.146788
                            },
                            WaypointAction = WaypointAction.Exit
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 7,
                    Name = "FB",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "FB entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.381974,
                                Longitude = -117.244699
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "FB Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.389651,
                                Longitude = -117.181909
                            },
                            WaypointAction = WaypointAction.Exit
                        }
                    }
                },
                new TollRoad()
                {
                    Id = 8,
                    Name = "Old 395",
                    Points = new List<TollRoadWaypoint>()
                    {
                        new TollRoadWaypoint
                        {
                            Name = "Old 395 entrance",
                            Location = new GeoLocation
                            {
                                Latitude = 33.392490,
                                Longitude = -117.172580
                            },
                            WaypointAction = WaypointAction.Enterce

                        },
                        new TollRoadWaypoint
                        {
                            Name = "Old 395 Exit",
                            Location = new GeoLocation
                            {
                                Latitude = 33.406353,
                                Longitude = -117.164065
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

            _dummyWaypoints = _dummyTollRoads.SelectMany(x => x.Points.Select(y => 
            {
                y.TollRoadId = x.Id;
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

        public TollRoadWaypoint FindNearestTollRoad(GeoLocation center, WaypointAction action)
        {
            return _distanceChecker.GetLocationFromRadius(center, GetAllWaypoints(action));
        }

        public IList<TollRoadWaypoint> GetAllWaypoints(WaypointAction action)
        {
            return _dummyWaypoints.Where(x => x.WaypointAction == action).ToList();
        }

        public TollRoadWaypoint FindNextTollRoad(TollRoadWaypoint point)
        {
            var points = _dummyTollRoads.FirstOrDefault(x => x.Id == point.TollRoadId);

            int index = points?.Points.IndexOf(point) + 1 ?? 0;

            return points?.Points.ElementAtOrDefault(index);
        }

        #endregion
    }
}