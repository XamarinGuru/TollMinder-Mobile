using System;
using Tollminder.Touch.Controls;
using UIKit;

namespace Tollminder.Touch.Helpers
{
    public class BoardFieldManager

    {
        private static ProfileButton profileButton;

        public static ProfileButton ButtonInitiaziler(nfloat _distanceBetweenTextAndImage)
        {
            profileButton = new ProfileButton(_distanceBetweenTextAndImage);
            return SetParameters();
        }

        public static ProfileButton ButtonInitiaziler(string text = null, UIImage icon = null)
        {
            profileButton = new ProfileButton();
            return SetParameters(text, icon);
        }

        private static ProfileButton SetParameters(string text = null, UIImage icon = null)
        {
            profileButton.ButtonText.Text = text;
            profileButton.ButtonText.Font = UIFont.BoldSystemFontOfSize(EnvironmentInfo.GetFontSizeForButtonTextField);
            profileButton.ButtonTextColor = UIColor.Black;
            profileButton.ButtonIcon = icon;
            profileButton.ButtonArrow = null;
            profileButton.BackgroundColor = UIColor.Cyan;
            return profileButton;
        }
    }
}
