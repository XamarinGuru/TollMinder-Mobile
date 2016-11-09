using System;
using CoreGraphics;
using UIKit;

namespace Tollminder.Touch.Extensions
{
    public static class TouchDrawing
    {
       public static UIImage ImageWithColor(UIColor color)
        {
            CGRect rect = new CGRect(0.0f, 0.0f, 1.0f, 1.0f);
            UIGraphics.BeginImageContext(rect.Size);
            UIImage image = null;
            using (CGContext context = UIGraphics.GetCurrentContext())
            {

                context.SetFillColor(color.CGColor);
                context.FillRect(rect);

                image = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
            }
            return image;
        }

        public static UIColor ToUIColor(this int hexValue)
        {
            return UIColor.FromRGB(
                (((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
                (((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
                (((float)(hexValue & 0xFF)) / 255.0f)
            );
        }
    }
}