using System;
using Tollminder.Touch.Controls;
using UIKit;

namespace Tollminder.Touch.Helpers
{
    public class RoundedButtonManager
    {
        private static RoundedButton _roundedButton;

        public static RoundedButton ButtonInitiaziler(nfloat _distanceBetweenTextAndImage)
        {
            _roundedButton = new RoundedButton(_distanceBetweenTextAndImage);
            return SetParameters();
        }

        public static RoundedButton ButtonInitiaziler(string text = null, UIImage image = null, UIImage pointer = null, UIColor textColor = null,
                                               UIColor backgroundColor = null, int linesNumber = 0)
        {
            _roundedButton = new RoundedButton();
            return SetParameters(text, image, pointer, textColor, backgroundColor, linesNumber);
        }

        private static RoundedButton SetParameters(string text = null, UIImage image = null, UIImage pointer = null, UIColor textColor = null,
                                               UIColor backgroundColor = null, int linesNumber = 0)
        {
            _roundedButton.ButtonText.Text = text;
            _roundedButton.ButtonText.Font = UIFont.BoldSystemFontOfSize(EnvironmentInfo.GetFontSizeForButtonTextField);
            _roundedButton.ButtonTextColor = textColor == null ? UIColor.White : textColor;
            _roundedButton.ButtonImage = image;
            _roundedButton.BackgroundColor = backgroundColor;
            _roundedButton.ButtonPointer = pointer != null ? pointer : null;
            return _roundedButton;
        }
    }
}
