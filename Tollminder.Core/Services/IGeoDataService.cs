using System;
using Tollminder.Core.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Tollminder.Core.Services
{
    public interface IGeoDataService : IGeoData
    {
        List<TollPointWithDistance> FindNearestEntranceTollPoints(GeoLocation center);
        List<TollPointWithDistance> FindNearestExitTollPoints(GeoLocation center);

        Task RefreshTollRoadsAsync(CancellationToken token);
    }
}

