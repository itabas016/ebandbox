using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    public class Category : ConnectableBase
    {
        protected override string MethodName
        {
            get { return "category"; }
        }

        protected override bool IsPagingFunction
        {
            get { return true; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["type"] = "0";
            this.NameValues["categoryid"] = "110";
            this.NameValues["rank"] = "1";
            base.AddAdditionalParams(parameters);
        }

        public override void TestLiveConnectable()
        {
            base.TestLiveConnectable();
        }
    }
}
