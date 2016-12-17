using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Services;
using Tollminder.Core.Services.Implementation;
using Tollminder.Core.ViewModels;

namespace Tollminder.Core
{
    public class CustomAppStart: MvxNavigatingObject, IMvxAppStart
    {
       public void Start(object hint = null)
        {
            var storedSettings = Mvx.Resolve<IStoredSettingsService>();
            //var dataBaseService = new DataBaseService();

            if (storedSettings.IsAuthorized)// && dataBaseService.IsTokenStillValid(storedSettings.AuthToken).Result)
            {
                ShowViewModel<HomeDebugViewModel>();
            }
            else
                ShowViewModel<LoginViewModel>();
        }
    }
}
