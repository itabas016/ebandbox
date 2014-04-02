using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Connectable
{
    /// <summary>
    /// 这个也没有给权限
    /// </summary>
    public class PostComment : ConnectableBase
    {
        protected override string MethodName
        {
            get { return "postcomment"; }
        }

        protected override bool IsPagingFunction
        {
            get { return false; }
        }

        public override void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["pkgid"] = "89571";
            base.AddAdditionalParams(parameters);
        }

        public override void TestEnvConnectable()
        {
            base.TestEnvConnectable();
        }
    }
}
