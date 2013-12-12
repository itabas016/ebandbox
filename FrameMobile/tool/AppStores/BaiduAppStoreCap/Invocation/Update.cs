using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    //POST
    public class Update: InvocationBase
    {
        protected override string MethodName
        {
            get { return "update"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["apilevel"] = "4";
            base.AddAdditionalParams(parameters);
        }
    }
}
