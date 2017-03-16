using System;
namespace Tollminder.Core.Models.PaymentData
{
    public class PayCreditCard : AddCreditCard
    {
        public string Cvv { get; set; }
    }
}
