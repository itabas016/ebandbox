using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class Suggest : InvocationBase
    {
        protected override string MethodName
        {
            get { return "sug"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["word"] = "";
            base.AddAdditionalParams(parameters);
        }
    }
}
