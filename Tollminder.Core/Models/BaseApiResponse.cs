using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    [JsonObject]
    public class BaseApiResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        public BaseApiResponse()
        {
        }

    }
}
