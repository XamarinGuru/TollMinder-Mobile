using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class ErrorApiResponse
    {
        [JsonProperty("err")]
        public string Message { get; set; }
    }
}
