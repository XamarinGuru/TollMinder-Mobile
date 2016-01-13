using System;

namespace Tollminder.Core.ServicesHelpers
{
	public interface ITrackFacade
	{
		void StartServices();
		void StopServices();
	}
}