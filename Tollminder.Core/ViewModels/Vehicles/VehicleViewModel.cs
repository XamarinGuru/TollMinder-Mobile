using System;
using Tollminder.Core.Services;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Models.DriverData;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;
using System.Threading.Tasks;

namespace Tollminder.Core.ViewModels.Vehicles
{
    public class VehicleViewModel : BaseViewModel
    {
        readonly IStoredSettingsService storedSettingsService;
        readonly IServerApiService serverApiService;

        public MvxCommand GoBakToVehicleListCommnad { get; set; }
        public MvxCommand CancelAddingCommnad { get; set; }
        public MvxCommand AddVehicleCommnad { get; set; }
        public Vehicle Vehicle { get; set; }

        public VehicleViewModel(IStoredSettingsService storedSettingsService, IServerApiService serverApiService)
        {
            this.storedSettingsService = storedSettingsService;
            this.serverApiService = serverApiService;

            GoBakToVehicleListCommnad = new MvxCommand(() => ShowViewModel<VehiclesDataViewModel>());
            AddVehicleCommnad = new MvxCommand(async () => await AddVehicleAsync());
            CancelAddingCommnad = new MvxCommand(async () =>
            {
                var result = await Mvx.Resolve<IUserInteraction>().ConfirmAsync("Are you sure you want to cancel?", "Warning", "Yes", "No");
                if (result)
                    ShowViewModel<VehiclesDataViewModel>();
            });
        }

        private Task AddVehicleAsync()
        {
            serverApiService.SaveVehicleAsync(Vehicle);
            return null;
        }
    }
}
