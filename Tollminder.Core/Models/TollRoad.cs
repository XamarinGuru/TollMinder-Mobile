using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
	[Table("TollRoads")]
	public class TollRoad
	{
		[PrimaryKey, AutoIncrement]
		public long Id { get; set; }
		public string Name { get; set; }
		[OneToMany(CascadeOperations = CascadeOperation.All)]
		public List<TollRoadWaypoint> Points { get; set; }
	}
}

