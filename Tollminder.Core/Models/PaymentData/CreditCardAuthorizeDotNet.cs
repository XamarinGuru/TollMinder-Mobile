using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tollminder.Core.Models.PaymentData
{
    public class CreditCardAuthorizeDotNet
    {
        [JsonProperty("customerProfileId")]
        public string CustomerProfileId { get; set; }
        [JsonProperty("paymentProfiles")]
        public PaymentProfile PaymentProfile { get; set; }
    }
}
