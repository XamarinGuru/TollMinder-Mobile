using System.Collections.Generic;

namespace Tollminder.Core.Services.ProfileData
{
    public interface ILoadResourceData<T>
    {
        List<T> GetData(string fileName = null);
    }
}
