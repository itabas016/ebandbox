using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Core
{
    public static class EncodeExtension
    {
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            var encodeStr = System.Convert.ToBase64String(plainTextBytes);
            encodeStr = encodeStr.Substring(0, encodeStr.IndexOf('='));
            return encodeStr;
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var remainder = base64EncodedData.Length % 4;
            if (remainder == 2)
            {
                base64EncodedData = base64EncodedData.Insert(base64EncodedData.Length, "==");
            }
            else if (remainder == 3)
            {
                base64EncodedData = base64EncodedData.Insert(base64EncodedData.Length, "=");
            }
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
