using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Domain
{
    public static class CommonExtensions
    {
        public static string NormalzieFileName(this string fileName)
        {
            return fileName.Replace("*", string.Empty).Replace(@"\", string.Empty).Replace("/", string.Empty).Replace(":", string.Empty).Replace("?", string.Empty).Replace("\"", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty).Replace("|", string.Empty);
        }
    }
}
