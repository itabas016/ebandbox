using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class CategoryList : InvocationBase
    {
        protected override string MethodName
        {
            get { return "cate"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            base.AddAdditionalParams(parameters);
        }
    }
}
