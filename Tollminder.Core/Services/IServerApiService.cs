using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Tollminder.Core.Models;
using System.Threading;

namespace Tollminder.Core.Services
{
	public interface IServerApiService
	{
        Task<IList<TollRoad>> RefreshTollRoads (long lastSyncDateTime, CancellationToken token);
	}
}