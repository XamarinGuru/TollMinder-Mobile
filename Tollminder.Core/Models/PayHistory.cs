using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class PayHistory
    {
        [JsonProperty(PropertyName = "_use")]
        public string UserId { get; set;}
        [JsonProperty(PropertyName = "_startWayPoint")]
        public string StartWayPoint { get; set; }
        [JsonProperty(PropertyName = "_endWayPoint")]
        public string EndWayPoint { get; set; }
        [JsonProperty(PropertyName = "_tollRoad")]
        public TollRoad TollRoad { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "tripDate")]
        public string TripDate { get; set; }
        [JsonProperty(PropertyName = "paymentDate")]
        public string PaymentDate { get; set; }
        [JsonProperty(PropertyName = "_transaction")]
        public string TransactionId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", TollRoad.Name, TripDate);
        }
    }
}
