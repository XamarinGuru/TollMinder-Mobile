using System;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
    public class SynchronisationService : ISynchronisationService
    {
        readonly IServerApiService serverApiService;
        readonly IStoredSettingsService storedSettingsService;
       
        public SynchronisationService()
        {
            serverApiService = Mvx.Resolve<IServerApiService>();
            storedSettingsService = Mvx.Resolve<IStoredSettingsService>();
        }

        public async Task DataSynchronisation()
        {
            if(storedSettingsService.IsDataSynchronized)
            {
                storedSettingsService.Profile = await serverApiService.GetProfile(storedSettingsService.ProfileId);
            }
            else
            {
                var result = await serverApiService.SaveProfile(storedSettingsService.Profile);
                if (result == System.Net.HttpStatusCode.OK)
                    storedSettingsService.IsDataSynchronized = true;
            }
        }
    }
}