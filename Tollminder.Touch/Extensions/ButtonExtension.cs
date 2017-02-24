using System;
using CoreGraphics;
using UIKit;

namespace Tollminder.Touch.Extensions
{
    public static class ButtonExtension
    {
        public static UIButton ButtonInitializer(this UIButton button, string title, UIControlState titleState, string imagePath, UIControlState imageState)
        {
            button.SetTitle(title, titleState);
            button.SetImage(UIImage.FromFile(imagePath), imageState);
            button.ClipsToBounds = false;
            button.Layer.CornerRadius = 5;
            button.Layer.ShadowColor = UIColor.Black.CGColor;
            button.Layer.ShadowOpacity = 0.1f;
            button.Layer.ShadowRadius = 1;
            button.Layer.ShadowOffset = new CGSize(1, 1);
            return button;
        }
    }
}
