using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QihooAppStoreCap.Invocation
{
    public class GetCategorys : InvocationBase
    {
        protected override string MethodName
        {
            get { return "getcat"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["channel"] = "tianyida";
            base.AddAdditionalParams(parameters);
        }
    }
}
