using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.ViewModels.UserProfile;
using Xamarin.Forms;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardViewModel : BaseViewModel
    {
        private MvxCommand backToProfileCommand;
        public ICommand BackToProfileCommand { get { return backToProfileCommand; } }
        private MvxCommand saveCreditCardCommand;
        public ICommand SaveCreditCardCommand { get { return saveCreditCardCommand; } }

        public CreditCardViewModel()
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
