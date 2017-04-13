using System;
using Tollminder.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Tollminder.Core.Services.GeoData
{
    public interface IGeoDataService : IGeoData
    {
        List<TollPointWithDistance> FindNearestTollPoints(GeoLocation center);
        List<TollPointWithDistance> FindNearestEntranceTollPoints(GeoLocation center);
        List<TollPointWithDistance> FindNearestExitTollPoints(GeoLocation center);

        Task RefreshTollRoadsAsync(CancellationToken token);
    }
}

