using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class Recommend : InvocationBase
    {
        protected override string MethodName
        {
            get { return "rec"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["package"] = "com.baidu.appsearch";
            this.NameValues["apilevel"] = "4";
            base.AddAdditionalParams(parameters);
        }
    }
}
