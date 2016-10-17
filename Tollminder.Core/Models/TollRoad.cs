using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
	[Table("TollRoads")]
    public class TollRoad : IEquatable<TollRoad>
	{
		[PrimaryKey, AutoIncrement]
		public long Id { get; set; }
		public string Name { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TollRoadWaypoint> WayPoints { get; set; }

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

