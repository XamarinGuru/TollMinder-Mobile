using System;

namespace Tollminder.Core.Models.PaymentData
{
    public class AddCreditCard
    {
        public string CardNumber { get; set; }

        public string Holder { get; set; }

        public DateTime ExpirationDate => new DateTime(ExpirationMonth, ExpirationYear, DateTime.DaysInMonth(ExpirationYear, ExpirationMonth));

        public int ExpirationMonth { get; set; } = 1;

        public int ExpirationYear { get; set; } = 1;

        // Need to scan credit card toll
        public static readonly AddCreditCard Empty = new AddCreditCard();
    }
}