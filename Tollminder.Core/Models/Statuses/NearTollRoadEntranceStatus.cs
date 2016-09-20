using System;
using System.Threading.Tasks;
using Tollminder.Core.Helpers;

namespace Tollminder.Core.Models.Statuses
{
	public class NearTollRoadEntranceStatus : BaseStatus
	{
		public override async Task<TollGeolocationStatus> CheckStatus ()
		{
			if (await SpeechToTextService.AskQuestion($"Are you entering {WaypointChecker.CurrentWaypoint.Name} tollroad?"))
			{
				WaypointChecker.Entrance = WaypointChecker.CurrentWaypoint;
				return TollGeolocationStatus.OnTollRoad;
			}

			return TollGeolocationStatus.NotOnTollRoad;
		}
	}
}

