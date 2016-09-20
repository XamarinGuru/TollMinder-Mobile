using System;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class NearTollRoadExitStatus : BaseStatus
	{
		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
			if (await SpeechToTextService.AskQuestion($"Are you exiting {WaypointChecker.CurrentWaypoint.Name} the tollroad?"))
				WaypointChecker.Exit = WaypointChecker.CurrentWaypoint;

			return TollGeolocationStatus.OnTollRoad;
		}
	}
}

