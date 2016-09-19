using System;
using MvvmCross.Platform.Converters;

namespace Tollminder.Droid.Converters
{
	public class InverseBoolConverter : MvxValueConverter<bool, bool>
	{
		protected override bool Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !value;
		} 
	}
}

