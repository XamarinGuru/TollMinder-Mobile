using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tollminder.Core.Models.PaymentData
{
    public class PaymentProfile
    {
        [JsonProperty("paymentProfileId")]
        public string PaymentProfileId { get; set; }
        [JsonProperty("cardNum")]
        public string CardNumber { get; set; }
    }
}
