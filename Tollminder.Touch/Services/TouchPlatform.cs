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
				//TODO exception run on ui thread.
				bool IsAppInForeground = true;
				UIApplication.SharedApplication.InvokeOnMainThread (() => {
					IsAppInForeground = UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active;
				});
				return IsAppInForeground;
			}
		}

		public void PauseMusic()
		{
			
		}

		public void PlayMusic()
		{
			
		}

		#endregion


	}
}

