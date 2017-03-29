using System;

namespace Tollminder.Core.Models.PaymentData
{
    public class AddCreditCard
    {
        public string UserId { get; set; }

        public string CreditCardNumber { get; set; }

        public string CardHolder { get; set; }

        public string ExpirationMonth { get; set; }

        public string ExpirationYear { get; set; }

        public string Cvv { get; set; }

        // Need to scan credit card toll
        public static readonly AddCreditCard Empty = new AddCreditCard();
    }
}