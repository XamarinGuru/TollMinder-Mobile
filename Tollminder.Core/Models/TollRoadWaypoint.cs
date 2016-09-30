using System;
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
	}

	public class TollRoadWaypointWithDistance : TollRoadWaypoint
	{
		public double Distance { get; set; }

		public TollRoadWaypointWithDistance()
		{
		}

		public  TollRoadWaypointWithDistance(TollRoadWaypoint tollRoadWaypoint)
		{
			Name = tollRoadWaypoint.Name;
			Id = tollRoadWaypoint.Id;
			TollRoadId = tollRoadWaypoint.TollRoadId;
			Location = tollRoadWaypoint.Location;
			WaypointAction = tollRoadWaypoint.WaypointAction;
		}
	}
}

