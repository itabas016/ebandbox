using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NCore;

namespace FrameMobile.Domain
{
    public static class StringExtensions
    {
        public static string SHA1Hash(this string str)
        {
            return Convert.ToBase64String(new SHA1CryptoServiceProvider()
                .ComputeHash(Encoding.ASCII.GetBytes(str.EncodeChineseChars())));
        }

        public static DateTime UTCStamp(this long timeStamp)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(timeStamp);
            return time;
        }

        public static double UnixStamp(this DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }
    }
}
