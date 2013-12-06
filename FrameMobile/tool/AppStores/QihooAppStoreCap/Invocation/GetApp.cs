using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QihooAppStoreCap
{
    public class GetApp : InvocationBase
    {
        protected override string MethodName
        {
            get { return "getApp"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["start"] = "0";
            this.NameValues["num"] = "100";
            this.NameValues["startTime"] = DateTime.Now.AddDays(-1).UnixStamp().ToString();
            this.NameValues["startTime"] = DateTime.Now.UnixStamp().ToString();
            base.AddAdditionalParams(parameters);
        }
    }
}
