using System;
namespace Tollminder.Core.Models.PaymentData
{
    public class TripCompleted
    {
        public string UserId { get; set; }
        public string StartWayPointId { get; set; }
        public string EndWayPointId { get; set; }
        public string TollRoadId { get; set; }
    }
}
