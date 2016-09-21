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
					Name = "Harkovkse metro exit",
					Location = new GeoLocation {
						Latitude = 50.403219, 
						Longitude =  30.667185
					},

					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "Kolektorna enterce",
					Location = new GeoLocation { 
						Latitude = 50.401042, 
						Longitude = 30.681115
					},

					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Kolektorna exit",
					Location = new GeoLocation {
						Latitude = 50.397919,
						Longitude = 30.676943
					},

					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "Poznaki Enterce",
					Location = new GeoLocation {
						Latitude = 50.399954,
						Longitude =  30.644416
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Poznaki Exit",
					Location = new GeoLocation {
						Latitude = 50.398490,
						Longitude = 30.634127
					},

					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "Osokorki Exit",
					Location = new GeoLocation {
						Latitude = 50.395579,
						Longitude = 30.616243
					},
					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "Osokorki Enter",
					Location = new GeoLocation {
						Latitude = 50.397062,
						Longitude = 30.626107
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
					Name = "Home",
					Location = new GeoLocation { 
						Latitude = 50.431920,
						Longitude = 30.515982
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "15s entrance",
					Location = new GeoLocation { 
						Latitude = 33.429156,
						Longitude = -117.147946
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "15s Exit",
					Location = new GeoLocation {
						Latitude = 33.387116,
						Longitude = -117.176185
					},
					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "15n entrance",
					Location = new GeoLocation {
						Latitude = 33.38717,
						Longitude = -117.173374
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "15n Exit",
					Location = new GeoLocation { 
						Latitude = 33.428762,
						Longitude = -117.146788
					},
					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "FB entrance",
					Location = new GeoLocation { 
						Latitude = 33.381974,
						Longitude = -117.244699
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "FB Exit",
					Location = new GeoLocation { 
						Latitude = 33.389651,
						Longitude = -117.181909
					},
					WaypointAction = WaypointAction.Exit
				},
				new TollRoadWaypoint {
					Name = "Old 395 entrance",
					Location = new GeoLocation { 
						Latitude = 33.392490, 
						Longitude = -117.172580
					},
					WaypointAction = WaypointAction.Enterce

				},
				new TollRoadWaypoint {
					Name = "Old 395 Exit",
					Location = new GeoLocation {
						Latitude = 33.406353, 
						Longitude = -117.164065
					},
					WaypointAction = WaypointAction.Enterce

				},
				new TollRoadWaypoint {
					Name = "Pechanga entrance",
					Location = new GeoLocation { 
						Latitude = 33.473324,
						Longitude = -117.12711
					},
					WaypointAction = WaypointAction.Enterce
				},
				new TollRoadWaypoint {
					Name = "Pechanga Exit",
					Location = new GeoLocation {
						Latitude = 33.455975, 
						Longitude = -117.103091
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