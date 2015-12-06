using System;
using Tollminder.Core.Services;

namespace Tollminder.Droid.Services
{
	public class DroidMotionActivity : IMotionActivity
	{
		#region IMotionActivity implementation

		public void StartDetection ()
		{
			throw new NotImplementedException ();
		}

		public void StopDetection ()
		{
			throw new NotImplementedException ();
		}

		public Tollminder.Core.Models.MotionType MotionType {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion


	}
}

