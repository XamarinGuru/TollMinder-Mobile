using System;
namespace Tollminder.Core.Models.GeoData
{
    public class TollGeoStatusResult
    {
        public TollGeolocationStatus TollGeolocationStatus { get; set; }
        public bool IsNeedToDoubleCheck { get; set; }
    }
}
