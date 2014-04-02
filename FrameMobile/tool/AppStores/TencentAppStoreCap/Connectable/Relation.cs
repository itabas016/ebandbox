using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    public class Relation:ConnectableBase
    {
        protected override string MethodName
        {
            get { return "relation"; }
        }

        protected override bool IsPagingFunction
        {
            get { return false; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["size"] = "10";
            this.NameValues["appid"] = "1112";
            base.AddAdditionalParams(parameters);
        }

        public override void TestEnvConnectable()
        {
            base.TestEnvConnectable();
        }

        public override void TestLiveConnectable()
        {
            this.NameValues["appid"] = "504403";

            base.TestLiveConnectable();
        }
    }
}
