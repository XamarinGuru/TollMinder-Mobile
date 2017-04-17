using System;
namespace Tollminder.Core.Models.GeoData
{
    public class TollGeoStatusResult
    {
        public TollPointWithDistance TollPointWithDistance { get; set; }
        public GeoLocation Location { get; set; }
        public TollGeolocationStatus TollGeolocationStatus { get; set; }
        public bool IsNeedToDoubleCheck { get; set; }
    }
}
