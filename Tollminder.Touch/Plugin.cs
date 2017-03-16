using System;
using MvvmCross.Platform.Plugins;
using Tollminder.Touch.Services;

namespace Tollminder.Touch
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            TouchCreditCardScanService.Initialize();
        }
    }
}
