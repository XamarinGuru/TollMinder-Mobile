using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Models.PaymentData;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Tollminder.Core.Services.Settings;
using MvvmCross.Platform;
using Chance.MvvmCross.Plugins.UserInteraction;

namespace Tollminder.Core.ViewModels.Payments
{
    public class AddCreditCardViewModel : BaseViewModel
    {
        readonly IPaymentProcessing paymentProcessing;
        readonly IStoredSettingsService storedSettingsService;

        public MvxCommand CloseAddCreditCardCommand { get; set; }
        public MvxCommand SaveCreditCardCommand { get; set; }

        public string CardHolder { get; set; }
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Card number is too short")]
        [Required]
        public string CreditCardNumber { get; set; } = "5424000000000015";
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Expiration month is too short")]
        [Required]
        public string ExpirationMonth { get; set; } = "5";
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Expiration year is too short")]
        [Required]
        public string ExpirationYear { get; set; } = "19";
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Security code is too short")]
        [Required]
        public string Cvv { get; set; } = "900";
        public string ZipCode { get; set; }

        public AddCreditCardViewModel(IPaymentProcessing paymentProcessing, IStoredSettingsService storedSettingsService)
        {
            this.paymentProcessing = paymentProcessing;
            this.storedSettingsService = storedSettingsService;

            CloseAddCreditCardCommand = new MvxCommand(async () => { await CloseAsync(); });
            SaveCreditCardCommand = new MvxCommand(async () => { await SaveCrediCardAsync(); });
        }

        public override void Start()
        {
            base.Start();
        }

        private async Task CloseAsync()
        {
            var answer = await Mvx.Resolve<IUserInteraction>().ConfirmAsync("Are you sure you want to leave this page?", "Warning");
            if (answer)
                ShowViewModel<CreditCardsViewModel>();
        }

        private async Task SaveCrediCardAsync()
        {
            if (ExpirationMonth.Length < 2)
                ExpirationMonth = "0" + ExpirationMonth;
            if (await Mvx.Resolve<IUserInteraction>().ConfirmAsync("Are sure you want to save your card? After saving you can't edit it.", "Warning", "Yes", "No"))
            {
                if (CheckField("Card Number", CreditCardNumber) && CheckField("Expiration Month", ExpirationMonth)
                   && CheckField("Expiration Year", ExpirationYear) && CheckField("Cvv", Cvv))
                {
                    var result = await paymentProcessing.AddCreditCardAsync(new AddCreditCard
                    {
                        UserId = storedSettingsService.ProfileId,
                        CreditCardNumber = this.CreditCardNumber,
                        ExpirationMonth = this.ExpirationMonth,
                        ExpirationYear = this.ExpirationYear,
                        CardCode = this.Cvv
                    });
                    if (result)
                        ShowViewModel<CreditCardsViewModel>();
                }
            }
        }

        private bool CheckField(string fieldName, string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
            {
                Mvx.Resolve<IUserInteraction>().AlertAsync(fieldName + " can't be null.", "Error");
                return false;
            }
            return true;
        }
    }
}
