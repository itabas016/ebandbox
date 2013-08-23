using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameMobile.Core
{
    public class DicSortALG
    {
        //Dic sort algorithm
        public static string ArraySort(string[] array)
        {
            Array.Sort(array);
            var sortResult = string.Join("", array);
            return sortResult;
        }
    }
}
