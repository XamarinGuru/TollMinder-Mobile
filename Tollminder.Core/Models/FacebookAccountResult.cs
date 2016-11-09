using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class FacebookAccountResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string PhotoUrl
        {
            get
            {
                return $"https://graph.facebook.com/{Id}/picture?type=large";
            }
        }
    }
}
