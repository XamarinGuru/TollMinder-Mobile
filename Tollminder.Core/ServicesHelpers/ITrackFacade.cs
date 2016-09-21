using System;
using System.Threading.Tasks;
using Tollminder.Core.Models;

namespace Tollminder.Core.ServicesHelpers
{
	public interface ITrackFacade
	{
		TollGeolocationStatus TollStatus { get; }

		Task<bool> StartServices();
		bool StopServices();
	}
}