﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaiduAppStoreCap.Service;

namespace BaiduAppStoreCap
{
    public abstract class InvocationBase
    {
        protected abstract string MethodName { get; }

        protected const string HOST = "http://m.baidu.com/api";

        protected ServiceProxy Proxy = new ServiceProxy();

        protected Dictionary<string, string> NameValues = new Dictionary<string, string>();

        public virtual void AddAdditionalParams(Dictionary<string, string> parameters)
        {
            this.NameValues["from"] = "1001816u";
            this.NameValues["token"] = "tianyida";
            this.NameValues["type"] = "app";

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
            sb.Append("?");
            sb.Append((from c in this.NameValues
                       let x = c.Key + "=" + c.Value
                       select x).Aggregate((a, b) => a + "&" + b)
                       );

            return sb.ToString();
        }

        public virtual string GetData(Dictionary<string, string> parameters)
        {
            var url = this.BuildUrl(HOST, this.MethodName, parameters);

            return this.Proxy.GetData(url);
        }
    }
}
