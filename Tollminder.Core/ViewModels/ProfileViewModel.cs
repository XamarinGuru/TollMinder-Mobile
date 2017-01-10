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
    public class ProfileViewModel : BaseViewModel
    {
        readonly ILoadResourceData<StatesData> loadResourceData;
        readonly int firstState = 0;

        public ProfileViewModel()
        {
            if (Mvx.CanResolve<ILoadResourceData<StatesData>>())
                loadResourceData = Mvx.Resolve<ILoadResourceData<StatesData>>();
            backHomeCommand = new MvxCommand(() => { ShowViewModel<HomeViewModel>(); });
            addLicenseCommand = new MvxCommand(() => { ShowViewModel<LicenseViewModel>(); });
            addCreditCardCommand = new MvxCommand(() => { ShowViewModel<CreditCardViewModel>(); });
            States = loadResourceData.GetData("Tollminder.Core.states.json");
            SelectedState = States[firstState];
        }

        public override void Start()
        {
            base.Start();
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
                SetProperty(ref selectedState, value);
                RaisePropertyChanged(() => StateAbbreviation); 
            }
        }

        public string StateAbbreviation
        {
            get { return SelectedState.Abbreviation; }
        }
    }
}
