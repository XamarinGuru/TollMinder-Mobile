using System;
using System.Text.RegularExpressions;
using MvvmCross.Platform;

namespace Tollminder.Core.Helpers
{
    public static class RegularExpressionHelper
    {
        public static string EmailRegexpr = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static bool ValidateRegExpression(this string text, string regExpr)
        {
            try
            {
                return new Regex(regExpr, RegexOptions.IgnoreCase).IsMatch(text);
            }
            catch (Exception e)
            {
                Mvx.Trace(e.Message, e.StackTrace);
            }

            return false;
        }
    }
}
