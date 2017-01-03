using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class StatesData
    {
        [JsonProperty("name")]
        public string Name { get; set;}
        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set;}

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Abbreviation);
        }
    }
}
