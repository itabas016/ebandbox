using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QihooAppStoreCap
{
    public class GetCategorys : InvocationBase
    {
        protected override string MethodName
        {
            get { return "getcat"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            base.AddAdditionalParams(parameters);
        }
    }
}
