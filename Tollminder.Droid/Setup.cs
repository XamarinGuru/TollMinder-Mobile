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
using System.Linq;

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
            Mvx.LazyConstructAndRegisterSingleton<IGPlusLoginService, DroidGPlusLoginService>();
            Mvx.LazyConstructAndRegisterSingleton<IHttpClientHandlerService, DroidHttpClientHandlerService>();
            Mvx.ConstructAndRegisterSingleton<IFacebookLoginService, DroidFacebookLoginService>();
		}

        protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = base.ValueConverterAssemblies.ToList();
                toReturn.Add(typeof(BoolInverseConverter).Assembly);
                toReturn.Add(typeof(Setup).Assembly);
                return toReturn;
            }
        }

		protected override void FillValueConverters(IMvxValueConverterRegistry registry)
		{
            base.FillValueConverters(registry);

            foreach(var assembly in ValueConverterAssemblies)
                foreach (var item in assembly.CreatableTypes().EndingWith("Converter"))
				    registry.AddOrOverwrite(item.Name, (IMvxValueConverter)Activator.CreateInstance(item));

            //registry.AddOrOverwrite("BoolInverseConverter", new BoolInverseConverter());
		}
    }
}