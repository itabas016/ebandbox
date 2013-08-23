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
    }
}
