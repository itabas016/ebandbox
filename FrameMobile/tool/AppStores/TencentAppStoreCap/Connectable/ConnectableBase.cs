using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TencentAppStoreCap.Service;

namespace TencentAppStoreCap.Connectable
{
    public abstract class ConnectableBase
    {
        protected ServiceProxy Proxy = new ServiceProxy();

        protected const string TEST_HOST = "http://petrelopen.kf0309.3g.qq.com/webapp_petrel_open/query.do?";
        protected const string Live_HOST = "http://interface.app.qq.com/open/query.do?";

        protected Dictionary<string, string> NameValues = new Dictionary<string, string>();

        protected virtual void AddCommonParams()
        {
            NameValues["app_type"] = "youle";
            NameValues["os_ver"] = "android2.3";
            NameValues["output"] = "json";
        }

        protected virtual void AddPagingParams()
        {
            NameValues["page_size"] = "10";
            NameValues["page_no"] = "1";
            NameValues["page_index"] = "1";
        }

        public virtual void AddAdditionalParams(Dictionary<string,string> parameters)
        {
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    NameValues[p.Key] = p.Value;
                }
            }
        }

        protected virtual string BuildUrl(string host, Dictionary<string, string> parameters)
        {
            this.NameValues.Clear();

            this.AddCommonParams();
            if (this.IsPagingFunction) this.AddPagingParams();

            this.AddAdditionalParams(parameters);

            this.NameValues["aid"] = this.MethodName;

            var sb = new StringBuilder();
            sb.Append(host);

            sb.Append((from c in this.NameValues
                       let x = c.Key + "=" + c.Value
                       select x).Aggregate((a, b) => a + "&" + b)
                       );

            return sb.ToString();
        }



        protected abstract string MethodName { get; }


        protected abstract bool IsPagingFunction { get; }


        #region Tests
        public virtual void TestEnvConnectable()
        {
            var url = this.BuildUrl(TEST_HOST, null);

            var ret = this.Proxy.GetData(url);

            Console.WriteLine(ret);
        }

        public virtual void TestLiveConnectable()
        {
            var url = this.BuildUrl(Live_HOST, null);

            var ret = this.Proxy.GetData(url);

            Console.WriteLine(ret);
        }
        #endregion

        #region Invoker

        public virtual string GetLiveData(Dictionary<string, string> parameters)
        {
            var url = this.BuildUrl(Live_HOST, parameters);

            return this.Proxy.GetData(url);
        }
        #endregion
    }

}
