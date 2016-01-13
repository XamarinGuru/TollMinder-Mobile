using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using Tollminder.Core.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Services.Implementation;
using Tollminder.Core.ServicesHelpers;
using Tollminder.Core.ServicesHelpers.Implementation;

namespace Tollminder.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
		public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();

			Mvx.LazyConstructAndRegisterSingleton<IGeoDataServiceAsync, GeoDataServiceAsync>();
			Mvx.LazyConstructAndRegisterSingleton<IHttpService, HttpService>();
			Mvx.LazyConstructAndRegisterSingleton<ITrackFacade, TrackFacade>();
        }
    }
}