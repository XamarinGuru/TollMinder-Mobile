using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;
using Xamarin.Forms;

namespace Tollminder.Core.ViewModels
{
    public class LicenseViewModel : BaseViewModel
    {
        readonly ILoadResourceData<StatesData> loadStatesData;
        readonly ILoadResourceData<string> loadVehicleData;
        readonly int firstElement = 0;
        
        public LicenseViewModel()
        {
            loadStatesData = Mvx.Resolve<ILoadResourceData<StatesData>>();
            States = loadStatesData.GetData("Tollminder.Core.states.json");
            SelectedState = States[firstElement];

            loadVehicleData = Mvx.Resolve<ILoadResourceData<string>>();
            VehicleClasses = loadVehicleData.GetData();
            SelectedVehicleClass = VehicleClasses[firstElement];

            backToProfileCommand = new MvxCommand(() => { ShowViewModel<ProfileViewModel>(); });
        }

        public override void Start()
        {
            base.Start();
        }

        private MvxCommand backToProfileCommand;
        public ICommand BackToProfileCommand { get { return backToProfileCommand; } }

        // States
        private List<StatesData> states;
        public List<StatesData> States
        {
            get { return states; }
            set
            {
                SetProperty(ref states, value);
                RaisePropertyChanged(() => States);
            }
        }

        private StatesData selectedState;
        public StatesData SelectedState
        {
            get { return selectedState; }
            set
            {
                SetProperty(ref selectedState, value);
                RaisePropertyChanged(() => StateAbbreviation);
            }
        }

        public string StateAbbreviation
        {
            get { return SelectedState.Abbreviation; }
        }

        // Vehicle classes
        private List<string> vehicleClasses;
        public List<string> VehicleClasses
        {
            get { return vehicleClasses; }
            set
            {
                SetProperty(ref vehicleClasses, value);
                RaisePropertyChanged(() => VehicleClasses);
            }
        }

        private string selectedVehicleClass;
        public string SelectedVehicleClass
        {
            get { return selectedVehicleClass; }
            set
            {
                SetProperty(ref selectedVehicleClass, value);
                RaisePropertyChanged(() => SelectedVehicleClass);
            }
        }
    }
}
