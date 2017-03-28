using MvvmCross.Core.ViewModels;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardsViewModel : BaseViewModel
    {
        public MvxCommand CloseCreditCardsCommand { get; set; }

        public CreditCardsViewModel()
        {
            CloseCreditCardsCommand = new MvxCommand(() => Close(this));
        }

        public override void Start()
        {
            base.Start();
        }
    }
}
