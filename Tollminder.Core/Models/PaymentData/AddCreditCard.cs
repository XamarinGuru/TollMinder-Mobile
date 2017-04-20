using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models.PaymentData
{
    public class AddCreditCard
    {
        [JsonProperty(PropertyName = "creditCardNumber")]
        public string CreditCardNumber { get; set; }

        //public string CardHolder { get; set; }

        [JsonProperty(PropertyName = "expirationMonth")]
        public string ExpirationMonth { get; set; }

        [JsonProperty(PropertyName = "expirationYear")]
        public string ExpirationYear { get; set; }

        [JsonProperty(PropertyName = "cardCode")]
        public string CardCode { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        // Need to scan credit card toll
        public static readonly AddCreditCard Empty = new AddCreditCard();
    }
}