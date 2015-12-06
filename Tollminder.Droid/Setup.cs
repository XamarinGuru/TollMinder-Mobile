using Android.Content;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using Tollminder.Core.Services;
using Tollminder.Droid.Services;

namespace Tollminder.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

		protected override void InitializePlatformServices ()
		{
			base.InitializePlatformServices ();
			Mvx.LazyConstructAndRegisterSingleton<IGeoLocationWatcher,DroidGeolocationWatcher> ();
			Mvx.LazyConstructAndRegisterSingleton<IMotionActivity,DroidMotionActivity> ();
		}
    }
}