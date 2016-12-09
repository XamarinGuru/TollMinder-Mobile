using System;
using System.Runtime.CompilerServices;
using Foundation;
using MvvmCross.Core.Platform;
using Tollminder.Core.Services;
using Tollminder.Touch.Services;

//[assembly: Xamarin.Forms.Dependency(typeof(CheckerAppFirstLaunch))]
namespace Tollminder.Touch.Services
{
    public class TouchCheckerAppFirstLaunch : ICheckerAppFirstLaunch
    {
        public TouchCheckerAppFirstLaunch() {}

        /// <summary>
        /// Check if app is running for the first time for updating data in SQLite
        /// </summary>
        /// <returns><c>true</c>, if app already launched once was ised, <c>false</c> if app launched for the first time.</returns>
        public bool IsAppAlreadyLaunchedOnce()
        {
            if (!NSUserDefaults.StandardUserDefaults.BoolForKey("HasLaunchedOnce"))
            {
                NSUserDefaults.StandardUserDefaults.SetBool(true, "HasLaunchedOnce");
                return false;
            }
            else
                return true;
        }
    }
}
