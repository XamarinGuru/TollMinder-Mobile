using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Api
{
    public interface IProfileSettingService
    {
        void SaveProfileInLocalStorage(Profile profile);
        Task<bool> SaveProfileAsync(Profile profile);
        Profile GetProfile();
        //bool IsDataSynchronized { get; set;}
    }
}
