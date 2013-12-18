using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAppStoreCap
{
    public class Suggest : InvocationBase
    {
        public string Word { get; set; }

        protected override string MethodName
        {
            get { return "sug"; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["action"] = this.MethodName;
            this.NameValues["word"] = this.Word;
            base.AddAdditionalParams(parameters);
        }
    }
}
