using UIKit;
using Tollminder.Touch.Services;
using Tollminder.Core.Services;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform;

namespace Tollminder.Touch
{
	public class Setup : MvxIosSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}
		
		public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
			: base(applicationDelegate, presenter)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new Core.App(this);
		}
		
		protected override IMvxTrace CreateDebugTrace()
		{
			return new DebugTrace();
		}

		protected override void InitializePlatformServices ()
		{
			base.InitializePlatformServices ();
            Mvx.LazyConstructAndRegisterSingleton<IInsightsService, TouchInsightsService>();
			Mvx.LazyConstructAndRegisterSingleton<IGeoLocationWatcher,TouchGeolocationWatcher> ();
			Mvx.LazyConstructAndRegisterSingleton<IMotionActivity,TouchMotionActivity> ();
			Mvx.LazyConstructAndRegisterSingleton<IPlatform,TouchPlatform> ();
			Mvx.LazyConstructAndRegisterSingleton<INotificationSender,TouchNotificationSender> ();
			Mvx.LazyConstructAndRegisterSingleton<ITextToSpeechService,TouchTextToSpeechService> ();
			Mvx.LazyConstructAndRegisterSingleton<ISpeechToTextService, TouchSpeechToTextService>();
			Mvx.LazyConstructAndRegisterSingleton<IStoredSettingsBase, TouchStoredSettingsBase>();
            Mvx.ConstructAndRegisterSingleton<IFacebookLoginService, TouchFacebookLoginService>();
            Mvx.LazyConstructAndRegisterSingleton<IGPlusLoginService, TouchGPlusLoginService>();
		}
	}
}
