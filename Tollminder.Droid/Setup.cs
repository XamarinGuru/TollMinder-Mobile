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
using Tollminder.Core.Converters;

namespace Tollminder.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
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
            Mvx.LazyConstructAndRegisterSingleton<IInsightsService, DroidInsightsService>();
			Mvx.LazyConstructAndRegisterSingleton<IGeoLocationWatcher,DroidGeolocationWatcher> ();
			Mvx.LazyConstructAndRegisterSingleton<IMotionActivity,DroidMotionActivity> ();
			Mvx.LazyConstructAndRegisterSingleton<INotificationSender,DroidNotificationSender> ();
			Mvx.LazyConstructAndRegisterSingleton<IPlatform, DroidPlatform> ();
			Mvx.LazyConstructAndRegisterSingleton<ITextToSpeechService, DroidTextToSpeechService> ();
			Mvx.LazyConstructAndRegisterSingleton<ISpeechToTextService, DroidSpeechToTextService>();
			Mvx.LazyConstructAndRegisterSingleton<IStoredSettingsBase, DroidStoredSettingsBase>();
		}

		protected override void FillValueConverters(IMvxValueConverterRegistry registry)
		{
			foreach (var item in CreatableTypes().EndingWith("Converter"))
				registry.AddOrOverwrite(item.Name, (IMvxValueConverter)Activator.CreateInstance(item));
            registry.AddOrOverwrite("BoolInverseConverter", new BoolInverseConverter());
		}
    }
}