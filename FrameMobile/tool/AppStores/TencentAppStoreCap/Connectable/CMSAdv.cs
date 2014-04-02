using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    public class CMSAdv : ConnectableBase
    {
        protected override string MethodName
        {
            get { return "cmsadv"; }
        }

        protected override bool IsPagingFunction
        {
            get { return false; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["cmsid"] = "223";
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
