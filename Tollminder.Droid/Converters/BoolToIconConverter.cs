using System;
using System.Globalization;
using Android.App;
using Android.Graphics.Drawables;
using MvvmCross.Platform.Converters;

namespace Tollminder.Droid.Converters
{
    public class BoolToIconConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format("res:{0}{1}", parameter, value ? "_active" : "_default");
        }
    }
}
