using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCore;

namespace BaiduAppStoreCap
{
    public class Content : InvocationBase
    {
        public long AppId { get; set; }

        public string PackageName { get; set; }

        public int VersionCode { get; set; }

        public string Sign { get; set; }

        protected override string MethodName
        {
            get { return "search"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["apilevel"] = "4";
            NecessaryParams();
            base.AddAdditionalParams(parameters);
        }

        private void NecessaryParams()
        {
            if (this.AppId <= 0)
            {
                this.NameValues["package"] = this.PackageName.MakeSureNotNull();
                this.NameValues["versioncode"] = this.VersionCode.ToString();
                //this.NameValues["signmd5"] = this.Sign;
            }
            else
            {
                this.NameValues["docid"] = this.AppId.ToString();
            }
        }
    }
}
