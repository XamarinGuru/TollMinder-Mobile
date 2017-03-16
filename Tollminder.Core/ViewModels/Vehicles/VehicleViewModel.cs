using System;
using Tollminder.Core.Services;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Models.DriverData;

namespace Tollminder.Core.ViewModels.Vehicles
{
    public class VehicleViewModel : BaseViewModel
    {
        readonly IStoredSettingsService storedSettingsService;
        readonly IServerApiService serverApiService;

        MvxCommand GoBakToVehicleListCommnad { get; set; }
        MvxCommand SaveVehicleCommnad { get; set; }
        private Vehicle Vehicle { get; set; }

        public VehicleViewModel(IStoredSettingsService storedSettingsService, IServerApiService serverApiService)
        {
            this.storedSettingsService = storedSettingsService;
            this.serverApiService = serverApiService;

            GoBakToVehicleListCommnad = new MvxCommand(() => ShowViewModel<VehiclesDataViewModel>());
            SaveVehicleCommnad = new MvxCommand(() => serverApiService.SaveVehicle(Vehicle));
        }
    }
}
