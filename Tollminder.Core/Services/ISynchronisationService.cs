using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface ISynchronisationService
    {
        Task DataSynchronisation();
        Task<bool> AuthorizeTokenSynchronisation();
    }
}
