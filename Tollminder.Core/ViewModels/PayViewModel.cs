using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace Tollminder.Core.ViewModels
{
    public class PayViewModel : BaseViewModel
    {
        private MvxCommand backToProfileCommand;
        public ICommand BackToProfileCommand { get { return backToProfileCommand; } }
        private MvxCommand goToCreditCardDataCommand;
        public ICommand GoToCreditCardDataCommand { get { return goToCreditCardDataCommand; } }

        public PayViewModel()
        {
            backToProfileCommand = new MvxCommand(() => { ShowViewModel<ProfileViewModel>(); });
            goToCreditCardDataCommand = new MvxCommand(() => { ShowViewModel<CreditCardViewModel>(); });
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
