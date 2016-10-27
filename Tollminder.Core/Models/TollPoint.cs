using System;
using MvvmCross.Platform;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Tollminder.Core.Models
{
    public class TollPoint: IEquatable<TollRoadWaypoint>
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(typeof(TollRoadWaypoint))]
        public long TollWaypointId { get; set; }
        public GeoLocation Location { get; set; }
        public WaypointAction WaypointAction { get; set; }

        public TollPoint()
        {

        }

        public TollPoint(string name, string location, string type)
        {
            Name = name;
            Location = new GeoLocation(location);
            WaypointAction enumType;
            if (Enum.TryParse(type, out enumType))
                WaypointAction = enumType;
            else
            {
                Mvx.Trace("Wrong action type");
                throw new Exception("Wrong action type");
            }
        }

        public bool Equals(TollRoadWaypoint other)
        {
            return Id == other.Id;
        }
    }

    public class TollPointWithDistance : TollPoint
    {
        public double Distance { get; set; }

        public TollPointWithDistance()
        {
        }

        public TollPointWithDistance(TollPoint tollPoint, double distance)
        {
            Name = tollPoint.Name;
            Id = tollPoint.Id;
            TollWaypointId = tollPoint.TollWaypointId;
            Location = tollPoint.Location;
            WaypointAction = tollPoint.WaypointAction;
            Distance = distance;
        }
    }
}

