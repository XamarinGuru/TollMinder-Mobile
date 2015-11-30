using System;
using Tollminder.Core.Models;

namespace Tollminder.Core.Services
{
	public interface IGeoLocationWatcher
	{
		GeoLocation GetMyPostion();
	}
}

