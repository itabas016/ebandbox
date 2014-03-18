using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCore;

namespace FrameMobile.Core
{
    public static class StringHelper
    {
        public static int GetWidth(this string resolution)
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

        public static int GetHeight(this string resolution)
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
    }
}
