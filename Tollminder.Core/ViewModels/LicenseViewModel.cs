using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        readonly IProfileSettingService profileSettingService;
        readonly int firstElement = 0;
        
        public LicenseViewModel()
        {
            loadStatesData = Mvx.Resolve<ILoadResourceData<StatesData>>();
            profileSettingService = Mvx.Resolve<IProfileSettingService>();
            profile = new Profile();
            driverLicense = new DriverLicense();

            States = loadStatesData.GetData("Tollminder.Core.states.json");
            
            loadVehicleData = Mvx.Resolve<ILoadResourceData<string>>();
            VehicleClasses = loadVehicleData.GetData();
            SelectedVehicleClass = VehicleClasses[firstElement];

            backToProfileCommand = new MvxCommand(() => {
                ShowViewModel<ProfileViewModel>(); 
            });
        }

        public override void Start()
        {
            base.Start();
            profile = profileSettingService.GetProfile();
            if (Profile.DriverLicense != null)
            {
                driverLicense = Profile.DriverLicense;
                SelectedVehicleClass = DriverLicense.VehicleClass;
                try
                {
                    SelectedState = States.Find(state => state.Name + " " + state.Abbreviation == Profile.DriverLicense.State);
                }
                catch (NullReferenceException ex)
                {
                    Debug.WriteLine(ex.Message);
                    SelectedState = States[firstElement];
                }
            }
        }

        public override void OnPause()
        {
            SaveDataBeforeYouLive();
            base.OnPause();
        }

        private void SaveDataBeforeYouLive()
        {
            Profile.DriverLicense = DriverLicense;
            profileSettingService.SaveProfile(Profile);
        }

        private MvxCommand backToProfileCommand;
        public ICommand BackToProfileCommand { get { return backToProfileCommand; } }

        private Profile profile;
        public Profile Profile
        {
            get { return profile; }
            set
            {
                SetProperty(ref profile, value);
            }
        }

        private DriverLicense driverLicense; 
        public DriverLicense DriverLicense
        {
            get { return driverLicense; }
            set{
                SetProperty(ref driverLicense, value);
            }
        }

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
                DriverLicense.State = value.ToString();
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
                DriverLicense.VehicleClass = value;
                RaisePropertyChanged(() => SelectedVehicleClass);
            }
        }
    }
}
