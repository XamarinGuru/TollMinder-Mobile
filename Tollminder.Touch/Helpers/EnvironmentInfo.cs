using System;
using Foundation;
using UIKit;

namespace Tollminder.Touch
{
    public static class EnvironmentInfo
    {
        public static bool IsForIOSNine
        {
            get { return UIDevice.CurrentDevice.CheckSystemVersion(9, 0); }
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

        public static nfloat GetGoogleButtonDistanceBetweenTextAndIcon
        {
            get { return GetScreenHeight > 640 ? 50 : 40; }
        }

        public static nfloat GetLabelDataWheelDistanceBetweenPlaceholderAndWheelText
        {
            get { return GetScreenHeight > 640 ? 3 : 2; }
        }

        public static nfloat GetValueForProfileViewLabel
        {
            get { return GetScreenWidth > 320 ? 20 : 10; }
        }

        public static nfloat GetMarginTopButtonsContainer
        {
            get { return GetScreenHeight > 640 ? 10 : -5; }
        }

        public static nfloat GetValueForGettingWidthButtonsContainer
        {
            get { return GetScreenWidth > 320 ? 0.8f : 0.76f; }
        }

        public static nfloat GetValueForGettingWidthRoadInformationContainer
        {
            get { return GetScreenWidth > 320 ? 0.76f : 0.682f; }
        }

        public static nfloat GetValueForSliderPositionX
        {
            get { return GetScreenWidth > 320 ? 310 : 248; }
        }

        public static nfloat GetValueForSliderPositionY
        {
            get { return GetScreenWidth > 320 ? 0 : 1; }
        }

        public static nfloat GetValueForFirstSliderPositionY
        {
            get { return GetScreenWidth > 320 ? 0 : -18; }
        }

        public static nfloat GetDistanceBetweenLabelAndTextNearestPoint
        {
            get { return GetScreenWidth > 320 ? 0.48f : 0.55f; }
        }

        public static nfloat GetDistanceBetweenLabelAndTextGeolocation
        {
            get { return GetScreenWidth > 320 ? 0.3f : 0.35f; }
        }

        public static nfloat GetDistanceBetweenLabelAndTextTollroad
        {
            get { return GetScreenWidth > 320 ? 0.2f : 0.25f; }
        }

        public static nfloat GetDistanceBetweenLabelAndTextStatus
        {
            get { return GetScreenWidth > 320 ? 0.18f : 0.2f; }
        }
    }
}

