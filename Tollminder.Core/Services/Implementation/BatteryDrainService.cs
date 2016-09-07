using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;

namespace Tollminder.Core.Services.Implementation
{
	public class BatteryDrainService : IBatteryDrainService
	{
		readonly IGeoLocationWatcher _geoWatcher;
		readonly IDistanceChecker _distanceChecker;
		readonly IGeoDataServiceAsync _geoDataServiceAsync;

		public BatteryDrainService()
		{
			_geoWatcher = Mvx.Resolve<IGeoLocationWatcher>();
			_distanceChecker = Mvx.Resolve<IDistanceChecker>();
			_geoDataServiceAsync = Mvx.Resolve<IGeoDataServiceAsync>();
		}

		public async Task NeedStopGpsTracking()
		{ 
			double? distance = _distanceChecker.GetMostClosestWaypoint(_geoWatcher.Location, (await _geoDataServiceAsync.FindNearGeoLocationsAsync(_geoWatcher.Location)).AsParallel().ToList())?.Distance;

			if (distance != null && (double)distance > 0)
			{
				
			}
		}
	}
}

