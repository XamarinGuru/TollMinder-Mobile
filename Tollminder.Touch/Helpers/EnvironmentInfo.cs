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
                 UIImage.FromFile(@"Images/main_background-667h@2x.png") :
                           UIImage.FromFile(@"Images/main_background-640h@2x.png");
            }
        }

        public static nfloat GetScreenSize
        {
            get { return UIScreen.MainScreen.Bounds.Height; }
        }

        public static nfloat GetFontSizeForButtonTextField
        {
            get { return GetScreenSize > 640 ? 15f : 14f; }
        }

        public static nfloat GetTrackingButtonWidth
        {
            get { return GetScreenSize > 640 ? 0.79f : 0.77f; }
        }

        public static nfloat GetTrackingButtonHeight
        {
            get { return GetScreenSize > 640 ? 0.78f : 0.85f; }
        }

        public static nfloat GetTrackingButtonDistanceBetweenTextAndImage
        {
            get { return GetScreenSize > 640 ? 25 : 20; }
        }

        public static nfloat GetRoundedButtonDistanceBetweenTextAndImage
        {
            get { return GetScreenSize > 640 ? 20 : 15; }
        }

        public static nfloat GetProfileButtonDistanceBetweenTextAndIcon
        {
            get { return GetScreenSize > 640 ? 60 : 50; }
        }
	}
}

