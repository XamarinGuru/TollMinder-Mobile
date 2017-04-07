using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Settings;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardsForPayViewModel : CreditCardsViewModel, IQueueItem
    {
        readonly IPaymentProcessing paymentProcessing;

        public ItemPriority Priority => ItemPriority.FirstAlways;
        public ICommand ItemSelectedCommand { get; set; }
        public ICommand CloseCreditCardsForPayCommand { get; set; }
        public string Amount { get; set; }

        public CreditCardsForPayViewModel(IPaymentProcessing paymentProcessing) : base(paymentProcessing)
        {
            this.paymentProcessing = paymentProcessing;
            ItemSelectedCommand = new MvxCommand<CreditCardAuthorizeDotNetViewModel>(selectedCard => PayForTrips(selectedCard));
        }

        private async Task PayForTrips(CreditCardAuthorizeDotNetViewModel selectedCard)
        {
            var result = await paymentProcessing.PayForTripAsync(new Models.PaymentData.PayForTrip()
            {
                UserId = Mvx.Resolve<IStoredSettingsService>().ProfileId,
                PaymentProfileId = selectedCard.CreditCard.PaymentProfileId,
                Amount = Amount
            });
            await Mvx.Resolve<IUserInteraction>().AlertAsync(result.Message, "Success");
            CloseCreditCardsForPayCommand.Execute(null);
        }
    }
}