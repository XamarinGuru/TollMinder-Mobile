using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Tollminder.Core.Models.PaymentData
{
    public class NotPayedTrip
    {
        [JsonProperty("trips")]
        public List<PayHistory> Trips { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}
