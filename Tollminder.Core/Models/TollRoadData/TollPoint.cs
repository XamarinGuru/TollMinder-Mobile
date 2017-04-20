using System;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
    public class TollPoint : IEquatable<TollRoadWaypoint>, IDatabaseEntry
    {
        [PrimaryKey]
        [JsonProperty("_id")]
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("_wayPoint")]
        [ForeignKey(typeof(TollRoadWaypoint))]
        public string TollWaypointId { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
        [JsonProperty("radius")]
        public double Radius { get; set; }
#if DEBUG
        [JsonProperty("interval")]
        public int Interval { get; set; }
#endif
        [Ignore]
        public GeoLocation Location { get; set; }
        [JsonProperty("action")]
        public WaypointAction WaypointAction { get; set; }

        public TollPoint()
        {
            Location = new GeoLocation();
        }

        public bool Equals(TollRoadWaypoint other)
        {
            return Id == other.Id;
        }
    }

    public class TollPointWithDistance : TollPoint
    {
        public double Distance { get; set; }

        public TollPointWithDistance()
        {
        }

        public TollPointWithDistance(TollPoint tollPoint, double distance)
        {
            Name = tollPoint.Name;
            Id = tollPoint.Id;
            TollWaypointId = tollPoint.TollWaypointId;
            Location = new GeoLocation
            {
                Latitude = tollPoint.Latitude,
                Longitude = tollPoint.Longitude,
                TollPointId = tollPoint.Id
            };
            WaypointAction = tollPoint.WaypointAction;
            Distance = distance;
        }
    }
}

