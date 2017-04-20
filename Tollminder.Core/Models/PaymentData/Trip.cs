using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Tollminder.Core.Services.Settings;

namespace Tollminder.Core.Models.PaymentData
{
    public class Trip : IQueueItem
    {
        [JsonProperty("tollRoadName")]
        public string TollRoadName { get; set; }
        [JsonProperty("paymentDate")]
        public string PaymentDate { get; set; }
        [JsonProperty("cost")]
        public string Cost { get; set; }
        [JsonProperty("_transaction")]
        public string Transaction { get; set; }

        public ItemPriority Priority => ItemPriority.Any;
    }
}
