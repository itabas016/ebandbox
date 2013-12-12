using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class Category : InvocationBase
    {
        public int CategoryId { get; set; }

        protected override string MethodName
        {
            get { return "cate"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["id"] = this.CategoryId.ToString();
            this.NameValues["apilevel"] = "4";
            this.NameValues["pn"] = "0";
            base.AddAdditionalParams(parameters);
        }
    }
}
