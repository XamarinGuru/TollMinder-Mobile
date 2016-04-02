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
		public Task<IList<TollRoadWaypoint>> GetWaypoints (CancellationToken token)
		{
			return _client.GetAsync<IList<TollRoadWaypoint>> (Host, token);  
		}
	}
}

