using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MvvmCross.Platform.Converters;

namespace Tollminder.Core.Converters
{
    public class ValidationConverter : MvxValueConverter<List<ValidationResult>, string>
    {
        public const string Name = "ValidationConverter";

        protected override string Convert(List<ValidationResult> value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var propertyPath = parameter as string;
            if (string.IsNullOrWhiteSpace(propertyPath))
                return null;

            if (!(value?.Any() ?? false))
                return null;

            return value?.FirstOrDefault(validationError =>
                        validationError.MemberNames.Any(memberName => memberName == propertyPath))
                        ?.ErrorMessage;
        }
    }
}
