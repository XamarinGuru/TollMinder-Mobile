using System;
using Tollminder.Core.Services;
using MvvmCross.Core.ViewModels;
namespace Tollminder.Core.ViewModels
{
    public class VehicleViewModel : BaseViewModel
    {
        IStoredSettingsService storedSettingsService;

        IMvxCommand BackVehiclesDataCommand { get; set; }

        public VehicleViewModel(IStoredSettingsService storedSettingsService)
        {
            //    this.storedSettingsService = storedSettingsService;
            //    BackVehiclesDataCommand = ShowViewModel<View>()
        }
    }
}
