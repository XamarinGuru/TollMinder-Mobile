using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models.PaymentData
{
    public class PayForTrip
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("paymentProfileId")]
        public string PaymentProfileId { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}
