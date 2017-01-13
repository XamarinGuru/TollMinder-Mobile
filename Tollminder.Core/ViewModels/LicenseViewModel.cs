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
            statesWheelCommand = new MvxCommand(() => {
                if (IsVehicleClassWheelHidden)
                    IsVehicleClassWheelHidden = false;
                IsStateWheelHidden = IsStateWheelHidden ? false : true;
            });
            vehicleClassesWheelCommand = new MvxCommand(() => { 
                if (IsStateWheelHidden)
                    IsStateWheelHidden = false;
                IsVehicleClassWheelHidden = IsVehicleClassWheelHidden ? false : true; 
            });
        }

        public override void Start()
        {
            base.Start();
        }

        private MvxCommand backToProfileCommand;
        public ICommand BackToProfileCommand { get { return backToProfileCommand; } }
        private MvxCommand statesWheelCommand;
        public ICommand StatesWheelCommand { get { return statesWheelCommand; } }
        private MvxCommand vehicleClassesWheelCommand;
        public ICommand VehicleClassesWheelCommand { get { return vehicleClassesWheelCommand; } }

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

        bool isStateWheelHidden;
        public bool IsStateWheelHidden
        {
            get { return isStateWheelHidden; }
            set
            {
                SetProperty(ref isStateWheelHidden, value);
                RaisePropertyChanged(() => IsStateWheelHidden);
            }
        }

        bool isVehicleClassWheelHidden;
        public bool IsVehicleClassWheelHidden
        {
            get { return isVehicleClassWheelHidden; }
            set
            {
                SetProperty(ref isVehicleClassWheelHidden, value);
                RaisePropertyChanged(() => IsVehicleClassWheelHidden);
            }
        }
    }
}
