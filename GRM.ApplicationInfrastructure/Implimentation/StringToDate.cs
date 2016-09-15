using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GRM.ApplicationInfrastructure
{
    public static class StringToDate
    {        
        public static DateTime ConvertToDateTime(this string dateString)
        {
            if (String.IsNullOrWhiteSpace(dateString))
            {
                return new DateTime();
            }

            var dateStringArray = dateString.Split(' ');
            if (dateStringArray.Length != 3)
            {
                return new DateTime();
            }

            dateStringArray[0] = Regex.Replace(dateStringArray[0], @"[^\d]", "").PadLeft(2, '0');

            var refinedDateString = dateStringArray[0] + "-" + dateStringArray[1] + "-" + dateStringArray[2];

            var refinedDate = FormatDate(refinedDateString, "dd-MMM-yyyy");

            if (refinedDate.Ticks != 0)
            {
                return refinedDate;
            }

            refinedDate = FormatDate(refinedDateString, "dd-MMMM-yyyy");

            if (refinedDate.Ticks != 0)
            {
                return refinedDate;
            }

            return new DateTime();

        }

        private static DateTime FormatDate(string dateString, string format)
        {
            DateTime date;

            if (DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }

            return new DateTime();
        }
    }
}
