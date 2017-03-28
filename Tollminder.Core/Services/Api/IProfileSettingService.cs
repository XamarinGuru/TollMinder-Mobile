using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Api
{
    public interface IProfileSettingService
    {
        void SaveProfile(Profile profile);
        Profile GetProfile();
        //bool IsDataSynchronized { get; set;}
    }
}
