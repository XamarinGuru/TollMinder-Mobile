using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class DriverLicense
    {
        [JsonProperty(PropertyName = "number")]
        public string LicensePlate { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "category")]
        public string VehicleClass { get; set; }
    }
}
