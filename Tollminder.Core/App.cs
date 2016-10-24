using System.Threading.Tasks;
using Tollminder.Core.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Services.Implementation;
using Tollminder.Core.ServicesHelpers;
using Tollminder.Core.ServicesHelpers.Implementation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform;
using MvvmCross.Core.Platform;
using Tollminder.Core.Helpers;

namespace Tollminder.Core
{
    public class App : MvxApplication
    {
        MvxSetup _setup;

        public App(MvxSetup setup)
        {
            _setup = setup;
            _setup.StateChanged += StateChanged;
        }

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();

			Mvx.LazyConstructAndRegisterSingleton<IGeoDataService, DummyDataSerivce>();
			Mvx.LazyConstructAndRegisterSingleton<IHttpService, HttpService>();
			Mvx.LazyConstructAndRegisterSingleton<INotifyService, NotifyService> ();
			Mvx.LazyConstructAndRegisterSingleton<IWaypointChecker, WaypointChecker> ();
			Mvx.LazyConstructAndRegisterSingleton<IDistanceChecker, DistanceChecker> ();
			Mvx.LazyConstructAndRegisterSingleton<ITrackFacade, TrackFacade>();
        }

        void StateChanged (object sender, MvxSetup.MvxSetupStateEventArgs e)
        {
            if (e.SetupState == MvxSetup.MvxSetupState.Initialized)
            {
                if (Mvx.Resolve<IStoredSettingsService>().GeoWatcherIsRunning)
                {
                    Task.Run(async () =>
                    {
                        Mvx.Resolve<ITrackFacade>().StopServices();

                        await Mvx.Resolve<ITrackFacade>().StartServices().ConfigureAwait(false);
                        Log.LogMessage("Autostart facade");
                         _setup.StateChanged -= StateChanged;
                        _setup = null;
                    });
                }
            }
        }
    }
}