using System;
using Windows.UI.Xaml.Data;

namespace LetsTalk.Common
{
    public class TimeAgoConverter : IValueConverter
    {
        private const string SuffixAgo = " ago";
        private const string SuffixFromNow = " from now";
        private const string Seconds = "less than a minute";
        private const string Minute = "about a minute";
        private const string Minutes = "{0} minutes";
        private const string Hour = "about an hour";
        private const string Hours = "about {0} hours";
        private const string Day = "a day";
        private const string Days = "{0} days";
        private const string Month = "about a month";
        private const string Months = "{0} months";
        private const string Year = "about a year";
        private const string Years = "{0} years";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is DateTime)) {
                return null;
            }

            var dateTime = (DateTime)value;
            var age = DateTime.Now.Subtract(dateTime);
            var delta = Math.Abs(age.TotalSeconds);

            string result;
            if (delta < 60)
                result = Seconds;
            else if (delta < 120)
                result = Minute;
            else if (delta < 2700) // 45 * 60
                result = String.Format(Minutes, age.Minutes);
            else if (delta < 5400) // 90 * 60
                result = Hour;
            else if (delta < 86400) // 24 * 60 * 60
                result = String.Format(Hours, age.Hours);
            else if (delta < 172800) // 48 * 60 * 60
                result = Day;
            else if (delta < 2592000) // 30 * 24 * 60 * 60
                result = String.Format(Days, age.Days);
            else if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                var months = System.Convert.ToInt32(Math.Floor((double) age.Days/30));
                result = months <= 1 ? Month : String.Format(Months, months);
            }
            else
            {
                var years = System.Convert.ToInt32(Math.Floor((double)age.Days / 365));
                result = years <= 1 ? Year : String.Format(Years, years);
            }

            return result + (age.TotalSeconds > 0 ? SuffixAgo : SuffixFromNow);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
