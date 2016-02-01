using Android.Content;
using Tollminder.Core.Services;
using Tollminder.Droid.Services;
using MvvmCross.Platform;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;

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
			Mvx.LazyConstructAndRegisterSingleton<INotificationSender,DroidNotificationSender> ();
			Mvx.LazyConstructAndRegisterSingleton<IPlatform, DroidPlatform> ();
			Mvx.ConstructAndRegisterSingleton<ITextToSpeechService, DroidTextToSpeechService> ();
		}
    }
}