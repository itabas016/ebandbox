using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    public class TopicList :ConnectableBase
    {
        protected override string MethodName
        {
            get { return "topiclist"; }
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
