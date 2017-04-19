using System;
using System.Text.RegularExpressions;
using MvvmCross.Platform.Converters;
using Tollminder.Core.Models;

namespace Tollminder.Core.Converters
{
    public class GeoLocationConverter : MvxValueConverter<GeoLocation, string>
    {
        protected override string Convert(GeoLocation value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format("{0}, {1}", CutStringToThreeSymbols(value.Latitude), CutStringToThreeSymbols(value.Longitude));
        }

        private double CutStringToThreeSymbols(double location)
        {
            string pattern = @"\d+(?:\.\d{1,4})?";
            var match = Regex.Match(location.ToString(), pattern);
            var cuttedLocation = double.Parse(match.Value);
            return location < 0 ? -cuttedLocation : cuttedLocation;
        }
    }
}
