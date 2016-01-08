using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore;
using System.Threading.Tasks;
using MessengerHub;
using Tollminder.Core.ViewModels;
using Tollminder.Core.Services;
using Tollminder.Core.Services.Implementation;

namespace Tollminder.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
		#region App Services Container

		public interface IAppServicesContainer {

			// messenger hub
			IMessengerHub MessengerHub { get; }
		}

		#endregion

		IAppServicesContainer _ServicesContainer;

		public App() {
			_ServicesContainer = new AppServicesContainer();
		}

		// this constructor is used by unit tests for mock services
		public App(IAppServicesContainer serviceContainer) {
			_ServicesContainer = serviceContainer;
		}

		// is the app in the foreground?
		public bool AppInForeground { get; set; }

        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();
				
            RegisterAppStart<ViewModels.HomeViewModel>();

			Mvx.LazyConstructAndRegisterSingleton<IMessengerHub, MessengerHub.MessengerHub>();
			Mvx.LazyConstructAndRegisterSingleton<IGeoDataServiceAsync, GeoDataServiceAsync>();

			// start any background services
			//InitializeSignificantLocationMonitoring();

			// really need to keep an eye on exceptions that happen inside of tasks
			TaskScheduler.UnobservedTaskException += ((sender, eventArgs) => {
				_ServicesContainer.MessengerHub.LogException(eventArgs.Exception);
				eventArgs.SetObserved();
			});

			// set application in foreground
			AppInForeground = true;
			_ServicesContainer.MessengerHub.Subscribe<AppInForegroundMessage>(m => {
				AppInForeground = m.Content;
			});

			// keep track of application state, and intercept all messages to view models
			_ServicesContainer.MessengerHub.InterceptAll((type, message) => {
				ViewModelBase.Publish(type, message);
			});
        }

		#region App Services Container implementation

		class AppServicesContainer : IAppServicesContainer{

			// new object to sync
			private object _SyncObject = new object();

			// messenger hub
			IMessengerHub _MessengerHub;
			public IMessengerHub MessengerHub
			{
				get
				{
					lock (_SyncObject)
					{
						if (_MessengerHub == null)
							_MessengerHub = Mvx.Resolve<IMessengerHub>();
					}
					return _MessengerHub;
				}
			}		
		}
		#endregion
    }
}