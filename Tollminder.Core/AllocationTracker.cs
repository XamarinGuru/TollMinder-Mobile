using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;

namespace Tollminder.Core
{
	public static class AllocationTracker
	{
		private static object _SyncObject;
		private static IDictionary<Type, int> _ActiveCount;
		private static LinkedList<Tuple<DateTime, WeakReference>> _References;

		static AllocationTracker()
		{
			_SyncObject = new object();
			_ActiveCount = new Dictionary<Type, int>();
			_References = new LinkedList<Tuple<DateTime, WeakReference>>();
		}

		// track changes
		public static void Track(object objectToTrack)
		{
			lock (_SyncObject)
			{
				var type = objectToTrack.GetType();
				_ActiveCount[type] = (_ActiveCount.ContainsKey(type) ? _ActiveCount[type] : 0) + 1;
				_References.AddLast(Tuple.Create(DateTime.Now, new WeakReference(objectToTrack)));
				Mvx.Trace("{0} CREATED [Total: {1}]", type.Name, _ActiveCount[type]);

				// cleanup
				for (var node = _References.First; node != null; node = node.Next)
					if (!node.Value.Item2.IsAlive)
						_References.Remove(node);
			}
		}

		// stop tracking
		public static void Untrack(object objectToTrack)
		{
			lock (_SyncObject)
			{
				var type = objectToTrack.GetType();
				_ActiveCount[type] = (_ActiveCount.ContainsKey(type) ? _ActiveCount[type] : 0) - 1;
				Mvx.Trace("{0} DELETED [Total: {1}]", type.Name, _ActiveCount[type]);
			}
		}

		// get current view models
		public static IEnumerable<T> GetTrackedObjectsAssignableTo<T>() where T : class
		{
			return GetTrackedObjectsAndTimesAssignableTo<T>().Select(t => t.Item2);
		}

		// get current view models
		public static IEnumerable<Tuple<DateTime, T>> GetTrackedObjectsAndTimesAssignableTo<T>() where T : class
		{
			lock (_SyncObject)
			{
				return _References
					.Select(t => Tuple.Create(t.Item1, t.Item2.Target as T))
					.Where(t => t.Item2 != null)
					.ToArray();
			}
		}
	}
}

