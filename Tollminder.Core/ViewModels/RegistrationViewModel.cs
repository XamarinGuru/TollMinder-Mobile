using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Tollminder.Core.Services;

namespace Tollminder.Core.ViewModels
{
    public class RegistrationViewModel: BaseViewModel
    {
        public RegistrationViewModel()
        {
            backToLoginViewCommand = new MvxCommand(() => { ShowViewModel<LoginViewModel>(); });
            registrationCommand = new MvxCommand(() => {});
        }

        public void Init(string name)
        {
            if(name != null)
                Mvx.Resolve<IProgressDialogManager>().CloseAndShowMessage("Welcome " + name, "Please continue registration.");
        }

        public override void Start()
        {
            base.Start();
        }

        string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set{ SetProperty(ref phoneNumber, value); }
        }

        MvxCommand backToLoginViewCommand;
        public ICommand BackToLoginViewCommand{ get { return backToLoginViewCommand; } }
        MvxCommand registrationCommand;
        public ICommand RegistrationCommand { get { return registrationCommand; } }
    }
}
