using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

        public static bool VerifyMd5Hash(this string input, string hash)
        {
            string hashOfInput = GetMD5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
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
            if (string.IsNullOrEmpty(resolution))
            {
                return 0;
            }
            //format : 320x480
            var lcdArray = resolution.ToLower().Split('x');
            var width = lcdArray[0].ToInt32();
            return width;
        }

        public static int GetResolutionHeight(this string resolution)
        {
            if (string.IsNullOrEmpty(resolution))
            {
                return 0;
            }
            //format : 320x480
            var lcdArray = resolution.ToLower().Split('x');
            var height = lcdArray[1].ToInt32();
            return height;
        }

        public static long TruncLong(this string input)
        {
            long result = 0;
            if (!string.IsNullOrEmpty(input))
            {
                input = Regex.Replace(input, @"[^\d]", "");
                if (Regex.IsMatch(input, @"^[+-]?\d*$"))
                {
                    result = Int64.Parse(input);
                }
            }
            return result;
        }

        public static List<int> GetIds(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var array = input.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (array != null && array.Length > 0)
                {
                    var ret = new List<int>();
                    for (int i = 0; i < array.Length; i++)
                    {
                        ret.Add(array[i].ToInt32());
                    }
                    return ret;
                }
            }
            return new List<int>() { 0 };
        }

        public static string GetString(this List<int> input)
        {
            if (input != null && input.Count > 0)
            {
                var sb = new StringBuilder();

                foreach (var item in input)
                {
                    sb.AppendFormat("{0},", item);
                }
                return sb.ToString().TrimEnd(',');
            }
            return string.Empty;
        }
    }
}
