using System.Threading.Tasks;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;
using Tollminder.Core.Models.PaymentData;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardAuthorizeDotNetViewModel : BaseViewModel
    {
        readonly IPaymentProcessing paymentProcessing;

        public CreditCardAuthorizeDotNet CreditCard { get; set; }
        public MvxCommand RemoveCreditCardCommand { get; set; }

        public CreditCardAuthorizeDotNetViewModel(CreditCardAuthorizeDotNet creditCard, IPaymentProcessing paymentProcessing)
        {
            CreditCard = creditCard;
            this.paymentProcessing = paymentProcessing;

            RemoveCreditCardCommand = new MvxCommand(async () => { await RemoveCreditCardAsync(); });
        }

        public override void Start()
        {
            base.Start();
        }

        private async Task RemoveCreditCardAsync()
        {
            var alertResult = await Mvx.Resolve<IUserInteraction>().ConfirmAsync("Are you sure you want to delete your credit card?", "Warning");
            if (alertResult)
                await paymentProcessing.RemoveCreditCardAsync(CreditCard.PaymentProfile.PaymentProfileId);
        }
    }
}
