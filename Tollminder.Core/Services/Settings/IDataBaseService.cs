using System.Collections.Generic;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Settings
{
    public interface IDataBaseService : IGeoData
    {
        void InsertOrUpdateAllTollRoads(IList<TollRoad> tollRoads);
        Task<IList<TollRoad>> GetTollRoadList();
    }
}
