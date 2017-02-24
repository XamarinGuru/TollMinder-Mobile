using System;
using Tollminder.Touch.Controls;
using UIKit;

namespace Tollminder.Touch.Helpers
{
    public class ProfileButtonManager
    {
        private static ProfileButton profileButton;

        public static ProfileButton ButtonInitiaziler(nfloat _distanceBetweenTextAndImage)
        {
            profileButton = new ProfileButton(_distanceBetweenTextAndImage, 0, 0, 0, 0);
            return SetParameters();
        }

        public static ProfileButton ButtonInitiaziler(string text = null, UIImage icon = null)
        {
            profileButton = new ProfileButton();
            return SetParameters(text, icon);
        }

        public static ProfileButton ButtonInitiaziler(nfloat _distanceBetweenTextAndImage, nfloat iconMarginLeft, nfloat iconHeight, nfloat iconWidth, nfloat textHeight, string text = null, UIImage icon = null, UIColor textColor = null,
                                                   UIColor backgroundColor = null, UIFont textFont = null)
        {
            profileButton = new ProfileButton(_distanceBetweenTextAndImage, iconMarginLeft, iconHeight, iconWidth, textHeight);
            return SetParameters(text, icon, textColor, backgroundColor, textFont);
        }

        private static ProfileButton SetParameters(string text = null, UIImage icon = null, UIColor textColor = null, 
                                                   UIColor backgroundColor = null, UIFont textFont = null)
        {
            profileButton.ButtonText.Text = text;
            profileButton.ButtonText.Font = textFont == null ? UIFont.BoldSystemFontOfSize(EnvironmentInfo.GetFontSizeForButtonTextField)
                : textFont;
            profileButton.ButtonTextColor = textColor == null ? UIColor.Black : textColor;
            profileButton.ButtonIcon = icon;
            profileButton.ButtonArrow = null;
            profileButton.BackgroundColor = backgroundColor == null ? UIColor.Cyan : backgroundColor;
            return profileButton;
        }
    }
}
