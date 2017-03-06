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
using System.Threading;
using Tollminder.Core.Utils.Slack;

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
            RegisterAppStart(new CustomAppStart());

            Mvx.LazyConstructAndRegisterSingleton<IWaypointChecker, WaypointChecker>();
            Mvx.LazyConstructAndRegisterSingleton<IDistanceChecker, DistanceChecker>();
            Mvx.LazyConstructAndRegisterSingleton<ITrackFacade, TrackFacade>();
        }

        async void StateChanged(object sender, MvxSetup.MvxSetupStateEventArgs e)
        {
            if (e.SetupState == MvxSetup.MvxSetupState.Initialized)
            {
                // need to test it
                await Mvx.Resolve<ITrackFacade>().CheckAreWeStillOnTheRoad();
                await Mvx.Resolve<ITrackFacade>().Initialize();
                _setup.StateChanged -= StateChanged;
                _setup = null;
            }
        }
    }
}