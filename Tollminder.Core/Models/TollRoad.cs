using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
	[Table("TollRoads")]
    public class TollRoad : IEquatable<TollRoad>, IDatabaseEntry
	{
		[PrimaryKey]
        [JsonProperty("_id")]
        public string Id { get; set; }
		public string Name { get; set; }
        [JsonProperty("_wayPoints")]
		[OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TollRoadWaypoint> WayPoints { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }
        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        public TollRoad()
        {
        }

        public TollRoad(string name, List<TollPoint> points)
        {
            Name = name;
            WayPoints = new List<TollRoadWaypoint>();
            foreach(var item in points)
            {
                WayPoints.Add(new TollRoadWaypoint()
                {
                    Name = item.Name,
                    //Location = item.Location,
                    TollPoints = new List<TollPoint>() { item }
                });
            };
        }

        public bool Equals(TollRoad other)
        {
            return Id == other.Id;
        }

        public override string ToString()
        {
            return string.Format("[TollRoad: Id={0}, Name={1}, WayPoints={2}]", Id, Name, WayPoints);
        }
    }
}

