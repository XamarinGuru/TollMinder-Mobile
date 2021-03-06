﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Models;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.ProfileData;
using Tollminder.Core.ViewModels.Payments;
using Tollminder.Core.ViewModels.Vehicles;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;

namespace Tollminder.Core.ViewModels.UserProfile
{
    public class ProfileViewModel : BaseViewModel
    {
        readonly ILoadResourceData<StatesData> loadResourceData;
        readonly IProfileSettingService profileSettingService;
        readonly int firstState = 0;

        public ProfileViewModel(ILoadResourceData<StatesData> loadResourceData, IProfileSettingService profileSettingService)
        {
            this.loadResourceData = loadResourceData;
            this.profileSettingService = profileSettingService;
            profile = new Profile();

            backHomeCommand = new MvxCommand(() => { ShowViewModel<HomeViewModel>(); });
            addLicenseCommand = new MvxCommand(() => { ShowViewModel<LicenseViewModel>(); });
            showCreditCardsCommand = new MvxCommand(() => { ShowCreditCards(); });

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
                SelectedState = States.Find(state => state.Name + " " + state.Abbreviation == Profile.State);
            }
            catch (NullReferenceException ex)
            {
                Debug.WriteLine(ex.Message);
                SelectedState = States[firstState];
            }
        }

        public override void OnPause()
        {
            if (Profile != null)
                profileSettingService.SaveProfileInLocalStorage(Profile);
            base.OnPause();
        }

        private async void ShowCreditCards()
        {
            if (await profileSettingService.SaveProfileAsync(Profile))
            {
                if (CheckField("FirstName", Profile.FirstName) && CheckField("Email", Profile.LastName) && CheckField("Email", Profile.Email)
                   && CheckField("Address", Profile.Address) && CheckField("City", Profile.City) && CheckField("State", Profile.State)
                   && CheckField("Zip Code", Profile.ZipCode))
                    ShowViewModel<CreditCardsViewModel>();
            }
        }

        private bool CheckField(string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                Mvx.Resolve<IUserInteraction>().AlertAsync(fieldName + " can't be null.", "Error");
                return false;
            }
            return true;
        }

        private MvxCommand backHomeCommand;
        public ICommand BackHomeCommand { get { return backHomeCommand; } }

        private MvxCommand addLicenseCommand;
        public ICommand AddLicenseCommand { get { return addLicenseCommand; } }

        private MvxCommand showCreditCardsCommand;
        public ICommand ShowCreditCardsCommand { get { return showCreditCardsCommand; } }

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
