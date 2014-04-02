using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    public class Update : ConnectableBase
    {
        protected override string MethodName
        {
            get { return "update"; }
        }

        protected override bool IsPagingFunction
        {
            get { return false; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            //base.AddAdditionalParams();
            base.AddAdditionalParams(parameters);
        }

        public override void TestLiveConnectable()
        {
            base.TestLiveConnectable();
        }
    }
}
