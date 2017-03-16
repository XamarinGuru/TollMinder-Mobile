using System;
namespace Tollminder.Core.Models.PaymentData
{
    public class CreditCardToken
    {
        public string CardLastDigits { get; set; }
        public string Token { get; set; }
    }
}
