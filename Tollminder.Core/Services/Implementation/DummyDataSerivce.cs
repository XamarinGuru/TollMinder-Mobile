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
		List<TollRoadWaypoint> _dummyWaypoints;
		IDistanceChecker _distanceChecker;

		public DummyDataSerivce (IDistanceChecker distanceChecker)
		{
			this._distanceChecker = distanceChecker;
			_dummyWaypoints = new List<TollRoadWaypoint> () {
				
				new TollRoadWaypoint {
					Name = "Harkovkse metro enterce",
					Location = new GeoLocation { 
						Latitude = 50.401130,
						Longitude =  30.652835
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Kolektorna enterce",
					Location = new GeoLocation { 
						Latitude = 50.397919, 
						Longitude = 30.676943
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Dnipro enterce",
					Location = new GeoLocation { 
						Latitude = 50.488499,
						Longitude = 30.475373
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Dnipro enterce",
					Location = new GeoLocation { 
						Latitude = 50.441272,
						Longitude = 30.559514
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Dryjba enterce",
					Location = new GeoLocation { 
						Latitude = 50.418160,
						Longitude = 30.544867
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Home",
					Location = new GeoLocation { 
						Latitude = 50.4014337,
						Longitude = 30.5309801
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = " Harkovske Enterce",
					Location = new GeoLocation { 
						Latitude = 50.414541,
						Longitude =  30.66253
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = " Brovarksy Enterce",
					Location = new GeoLocation { 
						Latitude = 50.464335,
						Longitude =  30.642740
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = " Leningradka Enterce",
					Location = new GeoLocation { 
						Latitude = 50.443157,
						Longitude =  30.626168
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = " Osokorki Enterce",
					Location = new GeoLocation { 
						Latitude = 50.398396,
						Longitude = 30.567202
					},
					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = " Osokorki Exit",
					Location = new GeoLocation { 
						Latitude = 50.394682,
						Longitude = 30.596464
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Home",
					Location = new GeoLocation { 
						Latitude = 50.431920,
						Longitude = 30.515982
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "15s",
					Location = new GeoLocation { 
						Latitude = 33.429156,
						Longitude = -117.147946
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "15n",
					Location = new GeoLocation { 
						Latitude = 33.428762,
						Longitude = -117.146788
					},
					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "15s",
					Location = new GeoLocation { 
						Latitude = 33.387116,
						Longitude = -117.176185
					},
					WaypointAction = WaypointAction.Enterce
				}, new TollRoadWaypoint {
					Name = "15n",
					Location = new GeoLocation { 
						Latitude = 33.38717,
						Longitude = -117.173374
					},
					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "FB",
					Location = new GeoLocation { 
						Latitude = 33.381974,
						Longitude = -117.244699
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "FB",
					Location = new GeoLocation { 
						Latitude = 33.387081,
						Longitude = -117.176838
					},
					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "Old 395",
					Location = new GeoLocation { 
						Latitude = 33.387188,
						Longitude = -117.172387
					},
					WaypointAction = WaypointAction.Enterce

				},
				new TollRoadWaypoint {
					Name = "Pechanga",
					Location = new GeoLocation { 
						Latitude = 33.473324,
						Longitude = -117.12711
					},
					WaypointAction = WaypointAction.Enterce
				}
			};
		}

		#region IGeoDataService implementation

		public ParallelQuery<TollRoadWaypoint> FindNearGeoLocations (GeoLocation center)
		{
			return _distanceChecker.GetLocationsFromRadius (center, _dummyWaypoints);
		}

		public TollRoadWaypoint FindNearGeoLocation (GeoLocation center, WaypointAction actionStatus)
		{			
			var nearLocations = _distanceChecker.GetLocationFromRadius (center, _dummyWaypoints.Where (x => x.WaypointAction == actionStatus).ToList ()); 
			return nearLocations;
		}

		public List<TollRoadWaypoint> GetWaypoints ()
		{
			return _dummyWaypoints;
		}

		public TollRoadWaypoint FindNearGeoLocation (GeoLocation center)
		{			
			return _distanceChecker.GetLocationFromRadius(center, _dummyWaypoints);
		}

		public void Update (TollRoadWaypoint geoLocation)
		{
			throw new NotImplementedException ();
		}

		public void Insert (TollRoadWaypoint geoLocation)
		{
			throw new NotImplementedException ();
		}

		public void Delete (TollRoadWaypoint geoLocation)
		{
			throw new NotImplementedException ();
		}

		public int Count {
			get {
				return _dummyWaypoints.Count;
			}
		}

		#endregion
	}
}