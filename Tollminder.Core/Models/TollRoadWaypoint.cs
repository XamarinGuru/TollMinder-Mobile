using System;
using SQLite.Net.Attributes;

namespace Tollminder.Core.Models
{
	public class TollRoadWaypoint : IEquatable<TollRoadWaypoint>
	{
		[PrimaryKey, AutoIncrement]
		public long Id { get; set; }
		public string Name { get; set; }
		public GeoLocation Location { get; set; }
		public WaypointAction WaypointAction { get; set; }


		#region IEquatable implementation
		public bool Equals (TollRoadWaypoint other)
		{			
			return Name == other.Name;
		}
		#endregion

		public override string ToString ()
		{
			if (Name == null) {
				return string.Empty;
			}
			return string.Format ("[TollRoadWaypoint: Id={0}, Name={1}, Location={2}, WaypointAction={3}]", Id, Name, Location, WaypointAction);
		}
	}
}

