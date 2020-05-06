using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tktclient.Utils
{
    class DateUtil
    {
        public static long UnixTime(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static DateTime GetDateTimeOnTs(long ts)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return startTime.AddSeconds(double.Parse(ts.ToString()));
        }

        public static DateTime GetDateTime(int yyyyMMdd)
        {
            return DateTime.ParseExact(yyyyMMdd.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
        }

        public static TimeSpan SecondToTimeSpan(int seconds)
        {
            var hour = seconds / 3600;
            var minute = (seconds % 3600) / 60;
            var sec = (seconds % 3600) % 60;
            return new TimeSpan(hour, minute, sec);
        }

        public static int TimeToInt(string time)
        {
            var sps = time.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);
            if (sps.Length == 1)
            {
                return int.Parse(sps[0]) * 3600;
            }

            if (sps.Length == 2)
            {
                return int.Parse(sps[0]) * 3600 + int.Parse(sps[1]) * 60;
            }

            if (sps.Length == 3)
            {
                return int.Parse(sps[0]) * 3600 + int.Parse(sps[1]) * 60 + int.Parse(sps[2]);
            }

            return 0;
        }
    }
}
