using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QihooAppStoreCap.Service;

namespace QihooAppStoreCap
{
    public abstract class InvocationBase
    {
        protected abstract string MethodName { get; }

        protected const string MOBILE_AIDE_URL = "http://openbox.mobilem.360.cn/third/";

        protected const string MOBILE_OPEN_INTF_AIDE_URL = "http://api.np.mobilem.360.cn/intf/";

        protected ServiceProxy Proxy = new ServiceProxy();

        protected Dictionary<string, string> NameValues = new Dictionary<string, string>();

        public virtual void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["from"] = "tianyida";

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    NameValues[p.Key] = p.Value;
                }
            }
        }

        protected virtual string BuildUrl(string host, string method, Dictionary<string, string> parameters)
        {
            this.NameValues.Clear();

            this.AddAdditionalParams(parameters);

            var sb = new StringBuilder();
            sb.Append(host);
            sb.Append(method);
            sb.Append("?");
            sb.Append((from c in this.NameValues
                       let x = c.Key + "=" + c.Value
                       select x).Aggregate((a, b) => a + "&" + b)
                       );

            return sb.ToString();
        }

        public virtual string GetData(Dictionary<string, string> parameters, bool openIntf = false)
        {
            var host = openIntf ? MOBILE_OPEN_INTF_AIDE_URL : MOBILE_AIDE_URL;

            var url = this.BuildUrl(host, this.MethodName, parameters);

            return this.Proxy.GetData(url);
        }
    }
}
