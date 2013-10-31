using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace QihooAppStoreCap.Service
{
    public class ServiceProxy
    {
        MD5Service MD5Service;

        public ServiceProxy()
        {
            MD5Service = new MD5Service("234232143f342");
        }

        public string Sign(string url)
        {
            var md5Hash = MD5Service.Enc(url);

            return string.Format("{0}&sign={1}", url, md5Hash);
        }

        public string GetData(string url)
        {
            url = Sign(url);

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            var content = response.Content;
            return content;
        }
    }
}
