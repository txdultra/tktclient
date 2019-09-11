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

        public static DateTime GetDateTime(long ts)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return startTime.AddSeconds(double.Parse(ts.ToString()));
        }

        public static DateTime GetDateTime(int yyyyMMdd)
        {
            return DateTime.ParseExact(yyyyMMdd.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
