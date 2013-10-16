﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FrameMobile.Core;
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

        public static string GetMD5Hash(this string input)
        {
            return Encrypt.GetMD5Hash(input);
        }

        public static DateTime UTCStamp(this long timeStamp)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(timeStamp);
            return time;
        }

        public static long UnixStamp(this DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return (long)intResult;
        }

        public static string DefaultValue(this string str)
        {
            var value = str.IsNullOrEmpty() ? string.Empty : str;

            return value;
        }

        public static List<int> ToInt32List(this List<string> list)
        {
            var result = new List<int>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    result.Add(item.ToInt32());
                }
            }
            return result;
        }

        public static int GetResolutionWidth(this string resolution)
        {
            //format : 320x480
            var lcdArray = resolution.ToLower().Split('x');
            var width = lcdArray[0].ToInt32();
            return width;
        }
    }
}
