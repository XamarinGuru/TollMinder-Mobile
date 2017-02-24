using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class PayHistory
    {
        [JsonProperty(PropertyName = "tollRoadName")]
        public string TollRoadName { get; set;}
        [JsonProperty(PropertyName = "cost")]
        public int Amount { get; set; }
        [JsonProperty(PropertyName = "paymentDate")]
        public string PaymentDate { get; set; }
        [JsonProperty(PropertyName = "_transaction")]
        public string TransactionId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -t {1}", TollRoadName, Amount);
        }
    }
}
