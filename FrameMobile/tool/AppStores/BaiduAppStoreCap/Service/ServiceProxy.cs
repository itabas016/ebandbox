using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace BaiduAppStoreCap.Service
{
    public class ServiceProxy
    {
        public string GetData(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            var content = response.Content;
            return content;
        }
    }
}
