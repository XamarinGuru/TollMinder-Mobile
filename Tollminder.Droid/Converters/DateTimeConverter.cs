using System;
using System.Globalization;
using Android.App;
using Android.Graphics.Drawables;
using MvvmCross.Platform.Converters;

namespace Tollminder.Droid.Converters
{
    public class DateTimeConverter : MvxValueConverter<DateTime, string>
    {
        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString(parameter.ToString());
        }
    }
}
