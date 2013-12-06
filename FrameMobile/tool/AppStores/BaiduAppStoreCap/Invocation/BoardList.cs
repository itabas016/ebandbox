using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class BoardList : InvocationBase
    {
        protected override string MethodName
        {
            get { return "board"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            base.AddAdditionalParams(parameters);
        }
    }
}
