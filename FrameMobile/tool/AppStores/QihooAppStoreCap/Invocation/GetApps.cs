using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QihooAppStoreCap.Invocation
{
    public class GetApps : InvocationBase
    {
        protected override string MethodName
        {
            get { return "getApps"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["start"] = "0";
            this.NameValues["num"] = "100";
            this.NameValues["startTime"] = DateTime.Now.AddDays(-1).UnixStamp().ToString();
            this.NameValues["from"] = "tianyida";
            base.AddAdditionalParams(parameters);
        }
    }
}
