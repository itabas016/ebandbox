using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using StructureMap;

namespace FrameMobile.Core
{
    [PluginFamily(IsSingleton = true)]
    public interface IRequestRepository
    {
        NameValueCollection Header { get; }

        NameValueCollection QueryString { get; }

        IDictionary HttpContextItems { get; }

        string UserHostName { get; }

        string RawUrl { get; }

        string ClientIP { get; }

        string SessionId { get; }

        string UserAgent { get; }

        string QueryUrl { get; }

        string PostedDataString { get; }

        string GetValueFromHeadOrQueryString(string key);
    }
}
