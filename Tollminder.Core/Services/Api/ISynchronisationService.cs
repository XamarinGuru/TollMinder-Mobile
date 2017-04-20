using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services.Api
{
    public interface ISynchronisationService
    {
        Task DataSynchronisationAsync();
        Task<bool> AuthorizeTokenSynchronisationAsync();
    }
}
