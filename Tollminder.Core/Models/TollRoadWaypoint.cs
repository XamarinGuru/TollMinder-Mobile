using System;
using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
	[Table("TollRoadWaypoints")]
	public class TollRoadWaypoint : IEquatable<TollRoadWaypoint>, IDatabaseEntry
	{
		[PrimaryKey]//, AutoIncrement]
        //public int DBId { get; set; }
        [JsonProperty("_id")]
		public string Id { get; set; }
        [JsonProperty("_tollRoad")]
		[ForeignKey(typeof(TollRoad))]
		public string TollRoadId { get; set; }
		public string Name { get; set; }
        [JsonProperty("_tollPoints")]
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TollPoint> TollPoints { get; set; }
        [JsonProperty("latitude")]
        public double Latitude { get; set;}

        [JsonProperty("longitude")]
        public double Longitude { get; set;}
       
        //[OneToOne(CascadeOperations = CascadeOperation.All)]
        //[JsonProperty("location")]
        [Ignore]
        public GeoLocation Location { get; set; }

		#region IEquatable implementation
		public bool Equals (TollRoadWaypoint other)
		{			
            return Id == other.Id;
		}
		#endregion

		//public override string ToString ()
		//{			
		//	return string.Format ("[TollRoadWaypoint: Id={0}, Name={1}, Location={2}, WaypointAction={3}]", Id, Name, Location);
		//}
	}
}

