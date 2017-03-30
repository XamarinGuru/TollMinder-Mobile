using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardsForPayViewModel : CreditCardsViewModel, IQueueItem
    {
        readonly IPaymentProcessing paymentProcessing;

        public ItemPriority Priority => ItemPriority.FirstAlways;
        public ICommand ItemSelectedCommand { get; set; }
        public MvxCommand CloseCreditCardsForPayCommand { get; set; }

        public CreditCardsForPayViewModel(IPaymentProcessing paymentProcessing, Action closeAction) : base(paymentProcessing)
        {
            this.paymentProcessing = paymentProcessing;

            ItemSelectedCommand = new MvxCommand<CreditCardAuthorizeDotNetViewModel>(selectedCard =>
            {

            });
            CloseCreditCardsForPayCommand = new MvxCommand(closeAction);
        }
    }
}