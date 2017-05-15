using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models.PaymentData
{
    public class TripResponse
    {
        [JsonProperty("_user")]
        public string UserId { get; set; }
        [JsonProperty("_startWayPoint")]
        public string StartWayPointId { get; set; }
        [JsonProperty("_endWayPoint")]
        public string EndWayPointId { get; set; }
        [JsonProperty("_tollRoad")]
        public string TollRoadId { get; set; }
        [JsonProperty("_rate")]
        public string Rate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("tripDate")]
        public string TripDate { get; set; }
    }
}
