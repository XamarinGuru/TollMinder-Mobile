using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Settings;
using System.Threading.Tasks;
using MvvmCross.Platform;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardsForPayViewModel : CreditCardsViewModel, IQueueItem
    {
        readonly IPaymentProcessing paymentProcessing;

        public ItemPriority Priority => ItemPriority.FirstAlways;
        public ICommand ItemSelectedCommand { get; set; }
        public MvxCommand CloseCreditCardsForPayCommand { get; set; }

        public CreditCardsForPayViewModel(IPaymentProcessing paymentProcessing, Action closeAction, string amount) : base(paymentProcessing)
        {
            this.paymentProcessing = paymentProcessing;
            ItemSelectedCommand = new MvxCommand<CreditCardAuthorizeDotNetViewModel>(selectedCard => PayForTrips(selectedCard, amount, closeAction));
            CloseCreditCardsForPayCommand = new MvxCommand(closeAction);
        }

        private async Task PayForTrips(CreditCardAuthorizeDotNetViewModel selectedCard, string amount, Action close)
        {
            await paymentProcessing.PayForTripAsync(new Models.PaymentData.PayForTrip()
            {
                UserId = Mvx.Resolve<IStoredSettingsService>().ProfileId,
                PaymentProfileId = selectedCard.CreditCard.PaymentProfileId,
                Amount = amount
            });
            close();
        }
    }
}