using System;
using System.Globalization;
using CoreGraphics;
using MvvmCross.Platform.Converters;
using UIKit;

namespace Tollminder.Touch.Converters
{
    public class GetSliderPageConverter : MvxValueConverter<bool, CGPoint>
    {
        protected override CGPoint Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? new CGPoint(EnvironmentInfo.GetValueForSliderPositionX, EnvironmentInfo.GetValueForSliderPositionY) : new CGPoint(0, 0);
        }
    }
}
