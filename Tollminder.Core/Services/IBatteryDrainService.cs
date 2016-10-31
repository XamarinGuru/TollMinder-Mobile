using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IBatteryDrainService
	{
        bool CheckGpsTrackingSleepTime(double distance);
	}
}

