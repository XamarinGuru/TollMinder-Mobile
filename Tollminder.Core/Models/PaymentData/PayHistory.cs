using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models.PaymentData
{
    public class PayHistory
    {
        [JsonProperty(PropertyName = "tollRoadName")]
        public string TollRoadName { get; set; }
        [JsonProperty(PropertyName = "cost")]
        public int Amount { get; set; }
        [JsonProperty(PropertyName = "_transaction")]
        public string TransactionId { get; set; }

        private string paymentDate;
        [JsonProperty(PropertyName = "paymentDate")]
        public string PaymentDate
        {
            get { return paymentDate; }
            set
            {
                paymentDate = DateTime.Parse(value).ToString("yy-MMM-dd ddd");
            }
        }
    }
}
