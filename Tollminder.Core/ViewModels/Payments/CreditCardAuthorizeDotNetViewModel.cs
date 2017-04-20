using System.Threading.Tasks;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;
using Tollminder.Core.Models.PaymentData;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using System;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardAuthorizeDotNetViewModel : BaseViewModel
    {
        readonly IPaymentProcessing paymentProcessing;

        public PaymentProfile CreditCard { get; set; }
        public MvxCommand RemoveCreditCardCommand { get; set; }
        private Action removeCreditCardFromList;

        public CreditCardAuthorizeDotNetViewModel(PaymentProfile creditCard, IPaymentProcessing paymentProcessing, Action removeCreditCardFromList)
        {
            CreditCard = creditCard;
            this.paymentProcessing = paymentProcessing;
            this.removeCreditCardFromList = removeCreditCardFromList;

            RemoveCreditCardCommand = new MvxCommand(async () => { await RemoveCreditCardAsync(); });
        }

        public override void Start()
        {
            base.Start();
        }

        private async Task RemoveCreditCardAsync()
        {
            if (await Mvx.Resolve<IUserInteraction>().ConfirmAsync("Are you sure you want to delete your credit card?", "Warning", "Yes", "Cancel"))
            {
                var result = await paymentProcessing.RemoveCreditCardAsync(CreditCard.PaymentProfileId);
                if (result)
                    removeCreditCardFromList();
                else
                    await Mvx.Resolve<IUserInteraction>().AlertAsync("Problem with removing your card. Please contact technical support.", "Error");
            }
        }
    }
}
