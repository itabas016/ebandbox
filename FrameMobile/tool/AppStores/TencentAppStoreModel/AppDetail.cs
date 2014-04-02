using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TencentAppStoreModel
{
    public class AppInfo
    {
        public int appid { get; set; }
        public int pkgid { get; set; }
        public int cpid { get; set; }
        public string cpname { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string apkurl { get; set; }
        public string apkver { get; set; }
        public string sysver { get; set; }
        public int categoryid { get; set; }
        public string category { get; set; }
        public string language { get; set; }
        public string copyright { get; set; }
        public string size { get; set; }
        public string author { get; set; }
        public string star { get; set; }
        public string voteCount { get; set; }
        public int downnum { get; set; }
        public DateTime updatetime { get; set; }
        public List<Label> labels { get; set; }
        public string detail { get; set; }
        public string logo { get; set; }
        public string istxapp { get; set; }
        public string fee { get; set; }
        public List<string> images { get; set; }
        public string permisssions { get; set; }
        public string packageName { get; set; }
        public int versionCode { get; set; }
    }
}
