using System;

namespace Tollminder.Core.Models.PaymentData
{
    public class AddCreditCard
    {
        public string UserId { get; set; }

        public string CreditCardNumber { get; set; }

        public string Holder { get; set; }

        public int ExpirationMonth { get; set; } = 1;

        public int ExpirationYear { get; set; } = 1;

        // Need to scan credit card toll
        public static readonly AddCreditCard Empty = new AddCreditCard();
    }
}