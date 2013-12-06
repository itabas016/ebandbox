using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QihooAppStoreCap
{
    public static class InvocationExtension
    {
        public static long UnixStamp(this DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return (long)intResult;
        }

        public static Dictionary<string, string> SortHightedByKey(this Dictionary<string, string> dic)
        {
            return Sort(dic, (x) => from pair in x orderby pair.Key descending select pair);
        }

        public static Dictionary<string, string> SortLowedByKey(this Dictionary<string, string> dic)
        {
            return Sort(dic, (x) => from pair in x orderby pair.Key select pair);
        }

        public static Dictionary<string, string> Sort(Dictionary<string, string> dic, Func<Dictionary<string, string>, IEnumerable<KeyValuePair<string, string>>> func)
        {
            Dictionary<string, string> dsistr = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                dsistr.Add(kvp.Key, kvp.Value);
            }
            var result = func(dsistr);
            Dictionary<string, string> dstr = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in result)
            {
                dstr.Add(kvp.Key, kvp.Value);
            }
            return dstr;
        }
    }
}
