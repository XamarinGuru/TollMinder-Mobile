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
                                Latitude = 50.399812, 
                                Longitude = 30.640644
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Poznaki Exit1",
                            Location = new GeoLocation
                            {
                                Latitude = 50.399394, 
                                Longitude = 30.638242
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Poznaki Exit2",
                            Location = new GeoLocation
                            {
                                Latitude = 50.399791,
                                Longitude = 30.635832
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
                            Name = "Home enterce",
                            Location = new GeoLocation
                            {
                                Latitude = 50.432576, 
                                Longitude = 30.513533
                            },
                            WaypointAction = WaypointAction.Enterce
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Home Exit1",
                            Location = new GeoLocation
                            {
                                Latitude = 50.431500, 
                                Longitude = 30.513552
                            },

                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Home Exit2",
                            Location = new GeoLocation
                            {
                                Latitude = 50.429534, 
                                Longitude = 30.513505
                            },

                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Home Exit3",
                            Location = new GeoLocation
                            {
                                Latitude = 50.428181, 
                                Longitude = 30.513559
                            },

                            WaypointAction = WaypointAction.Exit
                        },
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
                            Name = "15s Exit 1",
                            Location = new GeoLocation
                            {
                                Latitude = 33.421481, 
                                Longitude = -117.153897
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "15s Exit 2",
                            Location = new GeoLocation
                            {
                                Latitude = 33.411704, 
                                Longitude = -117.160756
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "15s Exit 3",
                            Location = new GeoLocation
                            {
                                Latitude = 33.401856, 
                                Longitude = -117.168674
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "15s Exit 4",
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
                            Name = "15n Exit 1",
                            Location = new GeoLocation
                            {
                                Latitude = 33.401856, 
                                Longitude = -117.168674
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "15n Exit 2",
                            Location = new GeoLocation
                            {
                                Latitude = 33.411704,
                                Longitude = -117.160756
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "15n Exit 3",
                            Location = new GeoLocation
                            {
                                Latitude = 33.421481, 
                                Longitude = -117.153897
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "15n Exit 4",
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
                            Name = "FB Exit 1",
                            Location = new GeoLocation
                            {
                                Latitude = 33.387464, 
                                Longitude = -117.236152
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                         new TollRoadWaypoint
                        {
                            Name = "FB Exit 2",
                            Location = new GeoLocation
                            {
                                Latitude = 33.389100, 
                                Longitude = -117.220234
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                         new TollRoadWaypoint
                        {
                            Name = "FB Exit 3",
                            Location = new GeoLocation
                            {
                                Latitude = 33.393478, 
                                Longitude = -117.210206
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                         new TollRoadWaypoint
                        {
                            Name = "FB Exit 4",
                            Location = new GeoLocation
                            {
                                Latitude = 33.389100,
                                Longitude = -117.220234
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                         new TollRoadWaypoint
                        {
                            Name = "FB Exit 5",
                            Location = new GeoLocation
                            {
                                Latitude = 33.387464, 
                                Longitude = -117.236152
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "FB Exit 6",
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
                            Name = "Old 395 Exit 1",
                            Location = new GeoLocation
                            {
                                Latitude = 33.399001, 
                                Longitude = -117.169927
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Old 395 Exit 2",
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
                            Name = "Pechanga Exit 1",
                            Location = new GeoLocation
                            {
                                Latitude = 33.468083, 
                                Longitude = -117.118997
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pechanga Exit 2",
                            Location = new GeoLocation
                            {
                                Latitude = 33.462371, 
                                Longitude = -117.111462
                            },
                            WaypointAction = WaypointAction.Exit
                        },
                        new TollRoadWaypoint
                        {
                            Name = "Pechanga Exit 3",
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