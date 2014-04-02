using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    public class TopicDetail : ConnectableBase
    {
        protected override string MethodName
        {
            get { return "topicdetail"; }
        }

        protected override bool IsPagingFunction
        {
            get { return true; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["topicid"] = "20066";
            base.AddAdditionalParams(parameters);
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
