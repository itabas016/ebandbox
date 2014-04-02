using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    public class Grading :ConnectableBase
    {
        protected override string MethodName
        {
            get { return "grading"; }
        }

        protected override bool IsPagingFunction
        {
            get { return true; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["type"] = "0";
            base.AddAdditionalParams(parameters);
        }

        public override void TestEnvConnectable()
        {
            base.TestEnvConnectable();
        }
    }
}
