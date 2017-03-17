using System;
using Tollminder.Core.Services;
using MvvmCross.Core.ViewModels;

namespace Tollminder.Core.ViewModels.Vehicles
{
    public class VehiclesDataViewModel : BaseViewModel
    {
        readonly IStoredSettingsService storedSettingsService;

        MvxCommand AddVehicleCommand { get; set; }

        public VehiclesDataViewModel(IStoredSettingsService storedSettingsService)
        {
            this.storedSettingsService = storedSettingsService;
            AddVehicleCommand = new MvxCommand(() => ShowViewModel<VehicleViewModel>());
        }
    }
}
