using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    /// <summary>
    /// 这个他们没有给我们权限
    /// </summary>
    public class GetComment : ConnectableBase
    {
        protected override string MethodName
        {
            get { return "getcomment"; }
        }

        protected override bool IsPagingFunction
        {
            get { return true; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["appid"] = "45592";
            base.AddAdditionalParams(parameters);
        }

        public override void TestEnvConnectable()
        {
            base.TestEnvConnectable();
        }
    }
}
