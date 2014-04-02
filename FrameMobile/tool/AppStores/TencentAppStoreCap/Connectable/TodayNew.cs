using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    public class TodayNew : ConnectableBase
    {
        protected override string MethodName
        {
            get { return "today"; }
        }

        protected override bool IsPagingFunction
        {
            get { return true; }
        }

        public override void TestEnvConnectable()
        {
            base.TestEnvConnectable();
        }

        public override void TestLiveConnectable()
        {
            base.TestLiveConnectable();
        }
    }
}
