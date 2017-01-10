using System.Collections.Generic;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface IDataBaseService : IGeoData
    {
        void InsertOrUpdateAllTollRoads(IList<TollRoad> tollRoads);
    }
}
