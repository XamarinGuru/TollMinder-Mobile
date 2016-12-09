using System;
using Foundation;
using UIKit;

namespace Tollminder.Touch
{
	public static class EnvironmentInfo
	{
		public static bool IsForIOSNine
		{			
			get { return UIDevice.CurrentDevice.CheckSystemVersion (9, 0); }
		}

        public static UIImage GetHomeBackground
        {
            get
            {
                return GetScreenSize > 640 ?
                 UIImage.FromFile(@"Images/home_background-667h@2x.png") :
                           UIImage.FromFile(@"Images/home_background-640h@2x.png");
            }
        }

        public static nfloat GetScreenSize
        {
            get { return UIScreen.MainScreen.Bounds.Height; }
        }

        public static nfloat GetFontSizeForButtonTextField
        {
            get { return GetScreenSize > 640 ? 15f : 12f; }
        }

        public static nfloat GetTrackingButtonWidth
        {
            get { return GetScreenSize > 640 ? 0.83f : 0.8f; }
        }

        public static nfloat GetTrackingButtonHeight
        {
            get { return GetScreenSize > 640 ? 1.0f : 1.05f; }
        }

        public static nfloat GetTrackingButtonDistanceBetweenTextAndImage
        {
            get { return GetScreenSize > 640 ? 40 : 30; }
        }

        public static nfloat GetRoundedButtonDistanceBetweenTextAndImage
        {
            get { return GetScreenSize > 640 ? 20 : 15; }
        }
	}
}

