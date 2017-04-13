using System;
using MvvmCross.Platform.Converters;

namespace Tollminder.Core.Converters
{
    public class GeoLocationConverter : MvxValueConverter<string, string>
    {
        private int startIndex = 0;
        private int numbersAfterComma = 4;

        protected override string Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var location = value.Split(',');
            return string.Format("{0}, {1}", CutStringToThreeSymbols(location[startIndex]), CutStringToThreeSymbols(location[1]));
        }

        private string CutStringToThreeSymbols(string location)
        {
            return location.Substring(startIndex, location.IndexOf('.') + numbersAfterComma);
        }
    }
}
