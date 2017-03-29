using System.Threading.Tasks;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;
using Tollminder.Core.Models.PaymentData;
using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.ViewModels.Payments
{
    public class CreditCardAuthorizeDotNetViewModel : BaseViewModel
    {
        readonly IServerApiService serverApiService;
        readonly IStoredSettingsService storedSettingsService;

        public CreditCardAuthorizeDotNet CreditCard { get; set; }
        public MvxCommand RemoveCreditCardCommand { get; set; }

        public CreditCardAuthorizeDotNetViewModel(CreditCardAuthorizeDotNet creditCard, IServerApiService serverApiService, IStoredSettingsService storedSettingsService)
        {
            CreditCard = creditCard;
            this.serverApiService = serverApiService;
            this.storedSettingsService = storedSettingsService;

            RemoveCreditCardCommand = new MvxCommand(async () => { await RemoveCreditCard(); });
        }

        public override void Start()
        {
            base.Start();
        }

        private async Task RemoveCreditCard()
        {
            var alertResult = await Mvx.Resolve<IUserInteraction>().ConfirmAsync("Are you sure you want to delete your credit card?", "Warning");
            if (alertResult)
                await serverApiService.RemoveCreditCardAsync(storedSettingsService.ProfileId, CreditCard.PaymentProfile.PaymentProfileId);
        }
    }
}
