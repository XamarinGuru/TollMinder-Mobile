using System;
using System.Diagnostics;
using MvvmCross.Platform;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services.Implementation
{
    public class ProfileSettingService : IProfileSettingService
    {
        readonly IStoredSettingsService storedSettingsService;
        
        public ProfileSettingService()
        {
            storedSettingsService = Mvx.Resolve<IStoredSettingsService>();
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
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.StackTrace);
            }
        }
    }
}
