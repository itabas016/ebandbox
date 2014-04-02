using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TencentAppStoreModel
{
    public class AppListItem
    {
        public string name { get; set; }
        public string type { get; set; }
        public int pkgid { get; set; }
        public int appid { get; set; }
        public string star { get; set; }
        public string resourceurl { get; set; }
        public string imgurl { get; set; }
        public string detail { get; set; }
        public string appver { get; set; }
        public int downnum { get; set; }
        public string packageName { get; set; }
        public string updatetime { get; set; }
        public int versionCode { get; set; }
        public string categoryname { get; set; }
        public int istxapp { get; set; }
        public string price { get; set; }
        public int cpid { get; set; }
        public string cpname { get; set; }
        public string voteCount { get; set; }
        public int categoryid { get; set; }
        public string size { get; set; }
    }
}
