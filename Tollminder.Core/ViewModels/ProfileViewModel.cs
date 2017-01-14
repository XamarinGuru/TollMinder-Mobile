using System;
using System.Collections.Generic;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Models;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        readonly ILoadResourceData<StatesData> loadResourceData;
        readonly ISynchronisationService synchronisationService;
        readonly IProfileSettingService profileSettingService;
        readonly int firstState = 0;

        public ProfileViewModel()
        {
            loadResourceData = Mvx.Resolve<ILoadResourceData<StatesData>>();
            profileSettingService = Mvx.Resolve<IProfileSettingService>();
            synchronisationService = Mvx.Resolve<ISynchronisationService>();
            Profile = new Profile();
            backHomeCommand = new MvxCommand(() => {
                synchronisationService.DataSynchronisation();
                ShowViewModel<HomeViewModel>(); 
            });
            addLicenseCommand = new MvxCommand(() => { ShowViewModel<LicenseViewModel>(); });
            addCreditCardCommand = new MvxCommand(() => { ShowViewModel<CreditCardViewModel>(); });
            statesWheelCommand = new MvxCommand(() => { 
                IsStateWheelHidden = IsStateWheelHidden ? false : true; 
            });

            States = loadResourceData.GetData("Tollminder.Core.states.json");
            SelectedState = States[firstState];
        }

        public override void Start()
        {
            base.Start();
            var result = profileSettingService.GetProfile();
            if (result != null)
                Profile = result;
        }

        public override void OnPause()
        {
            if (Profile != null)
                profileSettingService.SaveProfile(Profile);
            base.OnPause();
        }

        private MvxCommand backHomeCommand;
        public ICommand BackHomeCommand { get { return backHomeCommand; } }

        private MvxCommand addLicenseCommand;
        public ICommand AddLicenseCommand { get { return addLicenseCommand; } }

        private MvxCommand addCreditCardCommand;
        public ICommand AddCreditCardCommand { get { return addCreditCardCommand; } }

        private MvxCommand statesWheelCommand;
        public ICommand StatesWheelCommand { get { return statesWheelCommand; } }

        private List<StatesData> states;
        public List<StatesData> States
        {
            get { return states;}
            set 
            { 
                SetProperty(ref states, value);
                RaisePropertyChanged(() => States);
            }
        }

        private StatesData selectedState;
        public StatesData SelectedState
        {
            get { return new StatesData() { Name = Profile.State}; }
            set { 
                Profile.State = value.ToString();
                SetProperty(ref selectedState, value);
                RaisePropertyChanged(() => StateAbbreviation); 
            }
        }

        public string StateAbbreviation
        {
            get { return SelectedState.Abbreviation; }
        }

        private Profile profile;
        public Profile Profile
        {
            get { return profile; }
            set
            {
                SetProperty(ref profile, value);
                RaisePropertyChanged(() => Profile);
            }
        }

        private string firstName;
        public string FirstName
        {
            get { return Profile.FirstName; }
            set
            {
                Profile.FirstName = value;
                RaisePropertyChanged(() => FirstName);
            }
        }

        private string lastName;
        public string LastName
        {
            get { return Profile.LastName; }
            set
            {
                Profile.LastName = value;
                RaisePropertyChanged(() => LastName);
            }
        }

        private string email;
        public string Email
        {
            get { return Profile.Email; }
            set
            {
                Profile.Email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        private string address;
        public string Address
        {
            get { return Profile.Address; }
            set
            {
                Profile.Address = value;
                RaisePropertyChanged(() => Address);
            }
        }

        private string city;
        public string City
        {
            get { return Profile.City; }
            set
            {
                Profile.City = value;
                RaisePropertyChanged(() => City);
            }
        }

        private string zip;
        public string Zip
        {
            get { return Profile.ZipCode; }
            set
            {
                Profile.ZipCode = value;
                RaisePropertyChanged(() => Zip);
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
    }
}
