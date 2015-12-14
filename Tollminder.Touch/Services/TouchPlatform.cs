using System;
using Tollminder.Core.Services;
using UIKit;

namespace Tollminder.Touch.Services
{
	public class TouchPlatform : IPlatform
	{
		#region IPlatform implementation

		public bool IsAppInForeground {
			get {
				return UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active;
			}
		}

		#endregion


	}
}

