using System;
using System.Diagnostics;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.Services.Api
{
    public class ProfileSettingService : IProfileSettingService
    {
        readonly IStoredSettingsService storedSettingsService;

        public ProfileSettingService(IStoredSettingsService storedSettingsService)
        {
            this.storedSettingsService = storedSettingsService;
        }

        public Profile GetProfile()
        {
            return storedSettingsService.Profile;
        }

        public void SaveProfile(Profile profile)
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
