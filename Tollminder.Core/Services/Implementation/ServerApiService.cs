using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
	public class ServerApiService : IServerApiService
	{
		private readonly IHttpService _client;
		private readonly string Host;

		public ServerApiService (IHttpService client)
		{
			this._client = client;
		}

        public async Task<IList<TollRoad>> RefreshTollRoads (CancellationToken token)
		{
            var _dummyTollRoads = new List<TollRoad>()
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

            int roadCounter = 0;
            int waypointCounter = 0;
            int tollPointCounter = 0;
            foreach (var road in _dummyTollRoads)
            {
                road.Id = ++roadCounter;
                foreach (var waypoint in road.WayPoints)
                {
                    waypoint.Id = ++waypointCounter;
                    foreach (var tollpoint in waypoint.TollPoints)
                    {
                        tollpoint.Id = ++tollPointCounter;
                    }
                }
            }
            return _dummyTollRoads;

			//return _client.GetAsync<IList<TollRoadWaypoint>> (Host, token);  
		}
	}
}

