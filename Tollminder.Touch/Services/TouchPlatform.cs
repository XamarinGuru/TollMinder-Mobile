using System;
using Tollminder.Core.Services;

namespace Tollminder.Touch.Services
{
	public class TouchPlatform : IPlatform
	{
		#region IPlatform implementation

		public bool IsAppInForeground {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion


	}
}

