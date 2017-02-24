using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
    public interface IProfileSettingService
    {
        void SaveProfile(Profile profile);
        Profile GetProfile();
        //bool IsDataSynchronized { get; set;}
    }
}
