using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace Tollminder.Core.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private MvxCommand backHomeCommand;
        public ICommand BackHomeCommand { get { return backHomeCommand; } }
        private MvxCommand addLicenseCommand;
        public ICommand AddLicenseCommand { get { return addLicenseCommand; } }
        private MvxCommand addCreditCardCommand;
        public ICommand AddCreditCardCommand { get { return addCreditCardCommand; } }

        public ProfileViewModel()
        {
            backHomeCommand = new MvxCommand(() => { ShowViewModel<HomeViewModel>(); });
            addLicenseCommand = new MvxCommand(() => { ShowViewModel<LicenseViewModel>(); });
            addCreditCardCommand = new MvxCommand(() => { ShowViewModel<CreditCardViewModel>(); });   
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
