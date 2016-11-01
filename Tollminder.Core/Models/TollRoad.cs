using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
	[Table("TollRoads")]
    public class TollRoad : IEquatable<TollRoad>, IDatabaseEntry
	{
		[PrimaryKey, AutoIncrement]
		public long DBId { get; set; }
        public long Id { get; set; }
		public string Name { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<TollRoadWaypoint> WayPoints { get; set; }

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

