using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using UIKit;

namespace Tollminder.Touch.Converters
{
    public class GetPathToImageConverter : MvxValueConverter<bool, UIImage>
    {
        protected override UIImage Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagePath = string.Format(@"Images/homeView/ic_home_tracking{0}{1}", parameter, value ? "_active.png" : "_default.png");
            return UIImage.FromFile(imagePath);
        }
    }
}
