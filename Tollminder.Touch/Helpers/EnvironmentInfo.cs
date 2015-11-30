using System;
using UIKit;

namespace Tollminder.Touch
{
	public static class EnvironmentInfo
	{
		public static bool IsForIOSNine
		{			
			get { return UIDevice.CurrentDevice.CheckSystemVersion (9, 0); }
		}
	}
}

