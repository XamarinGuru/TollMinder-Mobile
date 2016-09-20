using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.ServicesHelpers
{
	public interface ITrackFacade
	{
		TollGeolocationStatus TollStatus { get; }

		void StartServices();
		void StopServices();
	}
}