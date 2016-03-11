using System.Threading.Tasks;
using Tollminder.Core.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Services.Implementation;
using Tollminder.Core.ServicesHelpers;
using Tollminder.Core.ServicesHelpers.Implementation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform;

namespace Tollminder.Core
{
    public class App : MvxApplication
    {
		public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();

			Mvx.LazyConstructAndRegisterSingleton<IGeoDataService, DummyDataSerivce>();
			Mvx.LazyConstructAndRegisterSingleton<IHttpService, HttpService>();
			Mvx.LazyConstructAndRegisterSingleton<ITrackFacade, TrackFacade>();
        }
    }
}