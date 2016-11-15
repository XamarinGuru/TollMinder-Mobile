using System;
namespace Tollminder.Core.Helpers
{
    public static class DateHelper
    {
        static DateTime UnixStartDateTime
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = UnixStartDateTime;
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static long UnixTime(this DateTime date, bool convertToUniversalTime = true)
        {
            var dt = (convertToUniversalTime && date != UnixStartDateTime) ? date.ToUniversalTime() : date;
            var timeSpan = (dt - UnixStartDateTime);
            return (long)timeSpan.TotalSeconds;
        }

        public static int GetDifference(DateTime olderDate, DateTime newerDate)
        {
            TimeSpan difference = newerDate - olderDate;
            return (difference.Days * 24 * 60 + difference.Hours * 60 + difference.Minutes);
        }

        public static string GetStringDifference(DateTime olderDate, DateTime newerDate)
        {
            TimeSpan difference = newerDate - olderDate;

            if (difference.Days < 30)
            {
                if (difference.Days < 1)
                    return difference.ToString("hh':'mm':'ss");
                else
                    return difference.ToString("d' day(s)'");
            }
            else
                return string.Format("{0} month(es)", difference.Days / 30);
        }
    }
}
