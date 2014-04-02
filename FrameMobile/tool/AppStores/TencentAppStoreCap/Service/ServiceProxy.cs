using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace TencentAppStoreCap.Service
{
    public class ServiceProxy
    {
        MD5Service MD5Service;

        public ServiceProxy()
        {
            MD5Service = new MD5Service("mf1dcf12df0v52747b80qkeecf0baese");
        }

        public string Sign(string url)
        {
            var md5Hash = MD5Service.Enc(url);

            return string.Format("{0}&api_sig={1}", url, md5Hash);
        }

        public string GetData(string url)
        {
            url = Sign(url);

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);

            var content = response.Content;
            if (!string.IsNullOrEmpty(content))
            {
                content = DESDecryptor.Decrypt(response.RawBytes, "YoulEChFnNeL&b#m%2!&!13y");
            }

            return content;
        }
    }
}
