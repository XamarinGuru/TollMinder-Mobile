using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;
using Cirrious.CrossCore;
using Tollminder.Touch.Services;
using Tollminder.Core.Services;

namespace Tollminder.Touch
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}
		
		public Setup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
			: base(applicationDelegate, presenter)
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
			Mvx.LazyConstructAndRegisterSingleton<IGeoLocationWatcher,TouchGeolocationWatcher> ();
			Mvx.LazyConstructAndRegisterSingleton<IMotionActivity,TouchMotionActivity> ();
		}
	}
}
