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
                return GetScreenHeight > 640 ?
                 UIImage.FromFile(@"Images/main_background-667h@2x.png") :
                           UIImage.FromFile(@"Images/main_background-640h@2x.png");
            }
        }

        public static nfloat GetScreenHeight
        {
            get { return UIScreen.MainScreen.Bounds.Height; }
        }

        public static nfloat GetScreenWidth
        {
            get { return UIScreen.MainScreen.Bounds.Width; }
        }

        public static nfloat GetFontSizeForButtonTextField
        {
            get { return GetScreenHeight > 640 ? 15f : 14f; }
        }

        public static nfloat GetTrackingButtonWidth
        {
            get { return GetScreenHeight > 640 ? 0.79f : 0.77f; }
        }

        public static nfloat GetTrackingButtonHeight
        {
            get { return GetScreenHeight > 640 ? 0.78f : 0.85f; }
        }

        public static nfloat GetTrackingButtonDistanceBetweenTextAndImage
        {
            get { return GetScreenHeight > 640 ? 25 : 20; }
        }

        public static nfloat GetRoundedButtonDistanceBetweenTextAndPointer
        {
            get { return GetScreenHeight > 640 ? 0.001f : 10; }
        }

        public static nfloat GetRoundedButtonDistanceBetweenTextAndImage
        {
            get { return GetScreenHeight > 640 ? 20 : 15; }
        }

        public static nfloat GetProfileButtonDistanceBetweenTextAndIcon
        {
            get { return GetScreenHeight > 640 ? 60 : 50; }
        }

        public static nfloat GetLabelDataWheelDistanceBetweenPlaceholderAndWheelText
        {
            get { return GetScreenHeight > 640 ? 3 : 2; }
        }

        public static nfloat GetValueForProfileViewLabel
        {
            get { return GetScreenWidth > 320 ? 20 : 10; }
        }
	}
}

