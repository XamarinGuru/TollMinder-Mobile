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

        public async Task<bool> AuthorizeTokenSynchronisation()
        {
            string result = await serverApiService.GetValidAuthorizeToken(storedSettingsService.ProfileId, storedSettingsService.AuthToken);
            if (storedSettingsService.AuthToken.Equals(result))
            {
                return storedSettingsService.IsAuthorized;
            }
            else
            {
                storedSettingsService.AuthToken = result;
                storedSettingsService.IsAuthorized = false;
                return false;
            }
        }

        public async Task DataSynchronisation()
        {
            if(storedSettingsService.IsDataSynchronized)
            {
                storedSettingsService.Profile = await serverApiService.GetProfile(storedSettingsService.ProfileId, storedSettingsService.AuthToken);
            }
            else
            {
                var result = await serverApiService.SaveProfile(storedSettingsService.ProfileId, storedSettingsService.Profile, storedSettingsService.AuthToken);
                if (result == System.Net.HttpStatusCode.OK)
                    storedSettingsService.IsDataSynchronized = true;
            }
        }
    }
}