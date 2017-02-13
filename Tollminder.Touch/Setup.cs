using UIKit;
using Tollminder.Touch.Services;
using Tollminder.Core.Services;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using Tollminder.Touch.Converters;
using Tollminder.Touch.Services.SpeechServices;

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
            Mvx.LazyConstructAndRegisterSingleton<ICheckerAppFirstLaunch, TouchCheckerAppFirstLaunch>();
			Mvx.LazyConstructAndRegisterSingleton<IPlatform,TouchPlatform> ();
			Mvx.LazyConstructAndRegisterSingleton<INotificationSender,TouchNotificationSender> ();
			Mvx.LazyConstructAndRegisterSingleton<ITextToSpeechService,TouchTextToSpeechService> ();
			Mvx.LazyConstructAndRegisterSingleton<ISpeechToTextService, TouchSpeechToTextService>();
			Mvx.LazyConstructAndRegisterSingleton<IStoredSettingsBase, TouchStoredSettingsBase>();
            Mvx.ConstructAndRegisterSingleton<IFacebookLoginService, TouchFacebookLoginService>();
            Mvx.LazyConstructAndRegisterSingleton<IFileManager, TouchFileManager>();
            Mvx.LazyConstructAndRegisterSingleton<IGPlusLoginService, TouchGPlusLoginService>();
            Mvx.LazyConstructAndRegisterSingleton<IHttpClientHandlerService, TouchHttpClientHandlerService>();
            Mvx.LazyConstructAndRegisterSingleton<IProgressDialogManager, TouchProgressDialogManager>();
		}

        //protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        //{
        //    registry.AddOrOverwrite("GetPathToImageConverter", new GetPathToImageConverter());
        //}
	}
}
