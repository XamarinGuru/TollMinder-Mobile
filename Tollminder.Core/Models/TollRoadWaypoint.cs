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
			return Name == other.Name && Location == other.Location;
		}
		#endregion

		public override string ToString ()
		{			
			return string.Format ("[TollRoadWaypoint: Id={0}, Name={1}, Location={2}, WaypointAction={3}]", Id, Name, Location, WaypointAction);
		}

//		public override int GetHashCode ()
//		{
//			return Name.GetHashCode () + Location.GetHashCode ();
//		}

//		public static bool operator == (TollRoadWaypoint t1, TollRoadWaypoint t2)
//		{
//			if (t1 == null) {
//				return false;
//			}
//			if (t2 == null) {
//				return false;
//			}
//			return t1.Equals (t2);
//		}
//
//		public static bool operator != (TollRoadWaypoint t1, TollRoadWaypoint t2)
//		{
//			if (t1 == null) {
//				return false;
//			}
//			if (t2 == null) {
//				return false;
//			}
//			return !t1.Equals (t2);
//		}
	}
}

