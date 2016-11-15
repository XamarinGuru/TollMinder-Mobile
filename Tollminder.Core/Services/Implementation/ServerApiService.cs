using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
    public class ServerApiService : ServerApiServiceBase, IServerApiService
	{
        public async Task<IList<TollRoad>> RefreshTollRoads (long lastSyncDateTime, CancellationToken token)
		{
            var r = await GetWithResultAsync<List<TollRoad>>(j => j, $"sync/{lastSyncDateTime}");
            return r;
		}


	}
}

