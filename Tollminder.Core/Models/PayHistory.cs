using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class PayHistoryTrips
    {
        public PayHistory[] trips { get; set;}
    }
    public class PayHistory
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set;}
        [JsonProperty(PropertyName = "_startWayPoint")]
        public string StartWayPoint { get; set; }
        [JsonProperty(PropertyName = "_endWayPoint")]
        public string EndWayPoint { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
        [JsonProperty(PropertyName = "_user")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "paymentDate")]
        public string PaymentDate { get; set; }
        [JsonProperty(PropertyName = "_transaction")]
        public string TransactionId { get; set; }
        [JsonProperty(PropertyName = "_tollRoad")]
        public TollRoad TollRoad { get; set; }
        [JsonProperty(PropertyName = "_rate")]
        public Rate Rate { get; set; }
        [JsonProperty(PropertyName = "tripDate")]
        public string TripDate { get; set; }

        //public override string ToString()
        //{
        //    return string.Format("{0} {1}", TollRoad.Name, TripDate);
        //}
    }
}
