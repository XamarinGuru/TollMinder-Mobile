using System;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
    [Table("DriverLicense")]
    public class DriverLicense //: IDatabaseEntry
    {
        //[PrimaryKey]
        //public string Id { get; set; }
        //[ForeignKey(typeof(Profile))]
        //public string ProfileId { get; set; }
        [JsonProperty(PropertyName = "licensePlate")]
        public string LicensePlate { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "vehicleClass")]
        public string VehicleClass { get; set; }
    }
}
