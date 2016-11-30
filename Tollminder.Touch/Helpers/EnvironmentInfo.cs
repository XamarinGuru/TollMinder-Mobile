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

        public static UIImage CheckDevice()
        {
            return UIScreen.MainScreen.Bounds.Height > 640 ? 
                 UIImage.FromFile(@"Images/home_background-667h@2x.png") :
                           UIImage.FromFile(@"Images/home_background-640h@2x.png");
        }
	}
}

