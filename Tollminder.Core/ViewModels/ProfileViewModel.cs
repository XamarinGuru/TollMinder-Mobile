using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        readonly IProfileSettingService profileSettingService;
        readonly int firstState = 0;

        public ProfileViewModel()
        {
            loadResourceData = Mvx.Resolve<ILoadResourceData<StatesData>>();
            profileSettingService = Mvx.Resolve<IProfileSettingService>();
            profile = new Profile();

            backHomeCommand = new MvxCommand(() => {
                ShowViewModel<HomeViewModel>(); 
            });
            addLicenseCommand = new MvxCommand(() => { ShowViewModel<LicenseViewModel>(); });
            addCreditCardCommand = new MvxCommand(() => { ShowViewModel<CreditCardViewModel>(); });

            States = loadResourceData.GetData("Tollminder.Core.states.json");
        }

        public override void Start()
        {
            base.Start();

            var result = profileSettingService.GetProfile();
            if (result != null)
                Profile = result;
            try
            {
                SelectedState = States.Find(state => state.Name+" "+state.Abbreviation == Profile.State);
            }
            catch(NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                SelectedState = States[firstState];
            }
        }

        public override void OnPause()
        {
            if (Profile != null)
                profileSettingService.SaveProfile(Profile);
            base.OnPause();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private MvxCommand backHomeCommand;
        public ICommand BackHomeCommand { get { return backHomeCommand; } }

        private MvxCommand addLicenseCommand;
        public ICommand AddLicenseCommand { get { return addLicenseCommand; } }

        private MvxCommand addCreditCardCommand;
        public ICommand AddCreditCardCommand { get { return addCreditCardCommand; } }

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
            get { return selectedState; }
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
            }
        }
    }
}
