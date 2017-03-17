using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface ISocialLoginServiceBase
    {
        void Initialize();
        void ReleaseResources();
        Task<SocialData> GetPersonDataAsync();
    }
}
