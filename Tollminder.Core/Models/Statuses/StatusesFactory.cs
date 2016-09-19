using System;
using System.Collections.Generic;

namespace Tollminder.Core.Models.Statuses
{
	public static class StatusesFactory
	{
		static readonly Dictionary<TollGeolocationStatus, BaseStatus> _pool;

		static StatusesFactory ()
		{
			_pool = new Dictionary<TollGeolocationStatus, BaseStatus>();
			_pool.Add (TollGeolocationStatus.NotOnTollRoad, new NotOnTollRoadStatus ());
			_pool.Add (TollGeolocationStatus.NearTollRoadEntrance, new NearTollRoadEntranceStatus ());
			_pool.Add (TollGeolocationStatus.OnTollRoad, new OnTollRoadStatus ());
			_pool.Add (TollGeolocationStatus.NearTollRoadExit, new NearTollRoadExitStatus ());

		}

		public static BaseStatus GetStatus (TollGeolocationStatus status)
		{
			BaseStatus statusObject = null;
			if (_pool.TryGetValue (status, out statusObject)) {
				return statusObject;
			}
			return null;
		}
	}
}

