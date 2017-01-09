using System;
using Newtonsoft.Json;

namespace Tollminder.Core.Models
{
    public class DriverLicense
    {
        [JsonProperty(PropertyName = "licensePlate")]
        public string LicensePlate { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "vehicleClass")]
        public string VehicleClass { get; set; }
    }
}
