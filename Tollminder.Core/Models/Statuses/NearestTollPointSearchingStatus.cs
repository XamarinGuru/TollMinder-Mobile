using System;
using System.Threading.Tasks;
using Tollminder.Core.Models.GeoData;

namespace Tollminder.Core.Models.Statuses
{
    public class NearestTollPointSearchingStatus : BaseStatus
    {
        public async override Task<TollGeoStatusResult> CheckStatus(TollGeoStatusResult tollGeoStatus)
        {
            var getPointWithResult = await CheckNearestPoint(tollGeoStatus.TollGeolocationStatus);
            var resultAfterChecking = getPointWithResult.IsNeedToDoubleCheck
                                                        ? await StatusesFactory.GetStatus(getPointWithResult.TollGeolocationStatus).CheckStatus(getPointWithResult)
                                                        : getPointWithResult;
            return resultAfterChecking;
        }

        public override bool CheckBatteryDrain()
        {
            return false;
        }
    }
}
