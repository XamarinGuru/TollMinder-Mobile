using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Settings;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;

namespace Tollminder.Core.Services.Api
{
    public class ProfileSettingService : IProfileSettingService
    {
        readonly IStoredSettingsService storedSettingsService;
        readonly IServerApiService serverApiService;

        public ProfileSettingService(IStoredSettingsService storedSettingsService, IServerApiService serverApiService)
        {
            this.storedSettingsService = storedSettingsService;
            this.serverApiService = serverApiService;
        }

        public Profile GetProfile()
        {
            return storedSettingsService.Profile;
        }

        public async Task<bool> SaveProfileAsync(Profile profile)
        {
            var result = await serverApiService.SaveProfileAsync(storedSettingsService.ProfileId, storedSettingsService.Profile, storedSettingsService.AuthToken);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
            {
                await Mvx.Resolve<IUserInteraction>().AlertAsync("Your personal data was not saved. Please try again later", result.StatusCode.ToString());
                return false;
            }
        }

        public void SaveProfileInLocalStorage(Profile profile)
        {
            try
            {
                storedSettingsService.Profile = profile;
                storedSettingsService.IsDataSynchronized = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }
    }
}
