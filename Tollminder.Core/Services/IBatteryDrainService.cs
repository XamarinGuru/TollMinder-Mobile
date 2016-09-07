using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services
{
	public interface IBatteryDrainService
	{
		Task NeedStopGpsTracking();
	}
}

