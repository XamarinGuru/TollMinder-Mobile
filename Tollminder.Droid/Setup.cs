using Android.Content;
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
using Tollminder.Droid.Views.Fragments;
using Tollminder.Core.Services.Settings;
using Tollminder.Core.Services.RoadsProcessing;
using Tollminder.Core.Services.Notifications;
using Tollminder.Core.Services.SpeechRecognition;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.SocialNetworks;
using Tollminder.Core.Services.ProfileData;
using Tollminder.Core.Services.GeoData;

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

        protected override void InitializePlatformServices()
        {
            base.InitializePlatformServices();
            Mvx.LazyConstructAndRegisterSingleton<IInsightsService, DroidInsightsService>();
            Mvx.LazyConstructAndRegisterSingleton<IGeoLocationWatcher, DroidGeolocationWatcher>();
            Mvx.LazyConstructAndRegisterSingleton<IMotionActivity, DroidMotionActivity>();
            Mvx.LazyConstructAndRegisterSingleton<INotificationSender, DroidNotificationSender>();
            Mvx.LazyConstructAndRegisterSingleton<IPlatform, DroidPlatform>();
            Mvx.LazyConstructAndRegisterSingleton<ITextToSpeechService, DroidTextToSpeechService>();
            Mvx.LazyConstructAndRegisterSingleton<ISpeechToTextService, DroidSpeechToTextService>();
            Mvx.LazyConstructAndRegisterSingleton<IStoredSettingsBase, DroidStoredSettingsBase>();
            Mvx.LazyConstructAndRegisterSingleton<IGPlusLoginService, DroidGPlusLoginService>();
            Mvx.LazyConstructAndRegisterSingleton<IHttpClientHandlerService, DroidHttpClientHandlerService>();
            Mvx.LazyConstructAndRegisterSingleton<IFileManager, DroidFileManager>();
            Mvx.ConstructAndRegisterSingleton<IFacebookLoginService, DroidFacebookLoginService>();
            Mvx.RegisterType<ICalendarDialog, CalendarDialog>();
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

            foreach (var assembly in ValueConverterAssemblies)
                foreach (var item in assembly.CreatableTypes().EndingWith("Converter"))
                    registry.AddOrOverwrite(item.Name, (IMvxValueConverter)Activator.CreateInstance(item));

            //registry.AddOrOverwrite("BoolInverseConverter", new BoolInverseConverter());
        }

        protected override void InitializeLastChance()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            base.InitializeLastChance();
        }

        protected override void FillTargetFactories(MvvmCross.Binding.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
        }
    }
}