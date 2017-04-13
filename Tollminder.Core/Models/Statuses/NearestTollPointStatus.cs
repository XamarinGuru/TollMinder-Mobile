using System;
using System.Threading.Tasks;
using Tollminder.Core.Models.GeoData;

namespace Tollminder.Core.Models.Statuses
{
    public class NearestTollPointStatus : BaseStatus
    {
        public override Task<TollGeoStatusResult> CheckStatus(TollGeolocationStatus tollGeoStatus)
        {
            return CheckNearestPoint(tollGeoStatus);
        }

        public override bool CheckBatteryDrain()
        {
            return false;
        }
    }
}
