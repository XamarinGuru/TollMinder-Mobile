using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
	[Table("TollRoadWaypoints")]
	public class TollRoadWaypoint : IEquatable<TollRoadWaypoint>
	{
		[PrimaryKey, AutoIncrement]
		public long Id { get; set; }
		[ForeignKey(typeof(TollRoad))]
		public long TollRoadId { get; set; }
		public string Name { get; set; }
		public GeoLocation Location { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TollPoint> TollPoints { get; set; }

		#region IEquatable implementation
		public bool Equals (TollRoadWaypoint other)
		{			
            return Id == other.Id;
		}
		#endregion

		public override string ToString ()
		{			
			return string.Format ("[TollRoadWaypoint: Id={0}, Name={1}, Location={2}, WaypointAction={3}]", Id, Name, Location);
		}
	}
}

