using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Common
{
    public static class TimeHelper
    {
        private static readonly TimeZoneInfo VNTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        public static DateTime GetVietnamCurrentTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, VNTimeZone);
        }
        public static DateTime ConvertToVietnamTime(DateTime utcTime)
        {
            if (utcTime.Kind == DateTimeKind.Unspecified)
                utcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, VNTimeZone);
        }
    }
}
