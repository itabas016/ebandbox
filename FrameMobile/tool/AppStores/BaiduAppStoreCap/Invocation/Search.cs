using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class Search : InvocationBase
    {
        public string Word { get; set; }

        public int PageNum { get; set; }

        protected override string MethodName
        {
            get { return "search"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["word"] = this.Word;
            this.NameValues["apilevel"] = "4";
            this.NameValues["rn"] = "50";
            this.NameValues["pn"] = PageNum.ToString();
            base.AddAdditionalParams(parameters);
        }
    }
}
