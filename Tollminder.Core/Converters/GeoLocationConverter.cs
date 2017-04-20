using System;
using MvvmCross.Platform.Converters;
using Tollminder.Core.Models;

namespace Tollminder.Core.Converters
{
    public class GeoLocationConverter : MvxValueConverter<GeoLocation, string>
    {
        private int countNumbersAfterDot = 4;

        protected override string Convert(GeoLocation value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format("{0}, {1}", CutStringToThreeSymbols(value.Latitude), CutStringToThreeSymbols(value.Longitude));
        }

        private double CutStringToThreeSymbols(double value)
        {
            return Math.Truncate(value * Math.Pow(10, countNumbersAfterDot)) / Math.Pow(10, countNumbersAfterDot);
        }
    }
}