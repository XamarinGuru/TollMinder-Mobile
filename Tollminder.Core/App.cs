using Tollminder.Core.ServicesHelpers;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform;
using MvvmCross.Core.Platform;
using Tollminder.Core.Services.RoadsProcessing;
using Tollminder.Core.Services.Api;

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
            Mvx.LazyConstructAndRegisterSingleton<IPaymentProcessing, PaymentProcessing>();
        }

        async void StateChanged(object sender, MvxSetup.MvxSetupStateEventArgs e)
        {
            if (e.SetupState == MvxSetup.MvxSetupState.Initialized)
            {
                // need to test it
                await Mvx.Resolve<ITrackFacade>().CheckAreWeStillOnTheRoadAsync();
                await Mvx.Resolve<ITrackFacade>().InitializeAsync();
                _setup.StateChanged -= StateChanged;
                _setup = null;
            }
        }
    }
}