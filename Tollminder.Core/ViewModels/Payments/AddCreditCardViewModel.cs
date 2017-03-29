using MvvmCross.Core.ViewModels;
using Tollminder.Core.Services.Api;
using Tollminder.Core.Models.PaymentData;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Tollminder.Core.Services.Settings;

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
        public string CreditCardNumber { get; set; }
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Expiration month is too short")]
        [Required]
        public string ExpirationMonth { get; set; }
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Expiration year is too short")]
        [Required]
        public string ExpirationYear { get; set; }
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Security code is too short")]
        [Required]
        public string Cvv { get; set; }

        public AddCreditCardViewModel(IPaymentProcessing paymentProcessing, IStoredSettingsService storedSettingsService)
        {
            this.paymentProcessing = paymentProcessing;
            this.storedSettingsService = storedSettingsService;

            CloseAddCreditCardCommand = new MvxCommand(() => Close(this));
            SaveCreditCardCommand = new MvxCommand(async () => { SaveCrediCard(); });
        }

        public override void Start()
        {
            base.Start();
        }

        private async Task SaveCrediCard()
        {
            await paymentProcessing.AddCreditCardAsync(new AddCreditCard
            {
                UserId = storedSettingsService.ProfileId,
                CardHolder = this.CardHolder,
                CreditCardNumber = this.CreditCardNumber,
                ExpirationMonth = this.ExpirationMonth,
                ExpirationYear = this.ExpirationYear,
                Cvv = this.Cvv
            });
        }
    }
}
