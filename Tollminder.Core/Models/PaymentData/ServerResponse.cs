using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace Tollminder.Core.Models.PaymentData
{
    public class ServerResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
