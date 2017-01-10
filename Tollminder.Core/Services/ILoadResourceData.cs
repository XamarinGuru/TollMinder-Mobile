using System;
using System.Collections.Generic;

namespace Tollminder.Core.Services
{
    public interface ILoadResourceData<T>
    {
        List<T> GetData(string fileName = null);
    }
}
