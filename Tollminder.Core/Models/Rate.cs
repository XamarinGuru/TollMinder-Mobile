using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class Rate
    {
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "_matrix")]
        public string Matrix { get; set; }
        [JsonProperty(PropertyName = "_startWayPoint")]
        public string StartWayPoint { get; set; }
        [JsonProperty(PropertyName = "_endWayPoint")]
        public string EndWayPoint { get; set; }
        [JsonProperty(PropertyName = "cost")]
        public int Amount { get; set; }
    }
}
