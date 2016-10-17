using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
    public class TollPoint
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(typeof(TollRoadWaypoint))]
        public long TollWaypointId { get; set; }
        public GeoLocation Location { get; set; }
        public WaypointAction WaypointAction { get; set; }
    }

    public class TollPointWithDistance : TollPoint
    {
        public double Distance { get; set; }

        public TollPointWithDistance()
        {
        }

        public TollPointWithDistance(TollPoint tollPoint)
        {
            Name = tollPoint.Name;
            Id = tollPoint.Id;
            TollWaypointId = tollPoint.TollWaypointId;
            Location = tollPoint.Location;
        }
    }
}

