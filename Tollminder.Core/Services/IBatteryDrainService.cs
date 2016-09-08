using System;
using System.Threading.Tasks;

namespace Tollminder.Core.Services
{
	public interface IBatteryDrainService
	{
		void CheckGpsTrackingSleepTime();
	}
}

