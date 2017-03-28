using MvvmCross.Core.ViewModels;

namespace Tollminder.Core.ViewModels.Payments
{
    public class AddCreditCardViewModel : BaseViewModel
    {
        public MvxCommand CloseAddCreditCardCommand { get; set; }
        public MvxCommand SaveCreditCardCommand { get; set; }

        public AddCreditCardViewModel()
        {
            CloseAddCreditCardCommand = new MvxCommand(() => Close(this));
            SaveCreditCardCommand = new MvxCommand(() => { ShowViewModel<CreditCardsViewModel>(); });
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
