using System;
using System.Threading.Tasks;
using MvvmCross.Platform;

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
            if (storedSettingsService.ProfileId == null || storedSettingsService.AuthToken == null)
                return false;
            else
            {
                string result = await serverApiService.GetValidAuthorizeToken();

                storedSettingsService.AuthToken = result;
                return storedSettingsService.IsAuthorized;
            }
        }

        public async Task DataSynchronisation()
        {
            if (storedSettingsService.IsDataSynchronized || storedSettingsService.Profile == null)
            {
                storedSettingsService.Profile = await serverApiService.GetProfile(storedSettingsService.ProfileId, storedSettingsService.AuthToken);
            }
            else
            {
                var result = await serverApiService.SaveProfile(storedSettingsService.ProfileId, storedSettingsService.Profile, storedSettingsService.AuthToken);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    storedSettingsService.IsDataSynchronized = true;
            }
        }
    }
}