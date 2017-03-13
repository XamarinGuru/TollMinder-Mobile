using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Tollminder.Core.ViewModels
{
    public class PayViewModel : BaseViewModel
    {
        private MvxCommand backToProfileCommand;
        public ICommand BackToProfileCommand { get { return backToProfileCommand; } }
        private MvxCommand saveCreditCardCommand;
        public ICommand SaveCreditCardCommand { get { return saveCreditCardCommand; } }

        public PayViewModel()
        {
            backToProfileCommand = new MvxCommand(() => { ShowViewModel<ProfileViewModel>(); });
            saveCreditCardCommand = new MvxCommand(() => { ShowViewModel<ProfileViewModel>(); });
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
