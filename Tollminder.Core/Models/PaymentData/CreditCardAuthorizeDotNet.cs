using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tollminder.Core.Models.PaymentData
{
    public class CreditCardAuthorizeDotNet : ServerResponse
    {
        [JsonProperty("customerProfileId")]
        public string CustomerProfileId { get; set; }
        [JsonProperty("paymentProfiles")]
        public List<PaymentProfile> PaymentProfile { get; set; }
    }
}
