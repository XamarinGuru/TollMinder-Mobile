using Android.Content;
using Tollminder.Core.Services;
using Tollminder.Droid.Services;
using MvvmCross.Platform;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.IoC;
using System;

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
			Mvx.ConstructAndRegisterSingleton<ISpeechToTextService, DroidSpeechToTextService>();
		}

		protected override void FillValueConverters(IMvxValueConverterRegistry registry)
		{
			foreach (var item in CreatableTypes().EndingWith("Converter"))
				registry.AddOrOverwrite(item.Name, (IMvxValueConverter)Activator.CreateInstance(item));
		}
    }
}