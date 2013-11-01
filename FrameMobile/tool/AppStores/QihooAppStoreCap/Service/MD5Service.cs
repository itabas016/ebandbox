using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using QihooAppStoreCap.Invocation;

namespace QihooAppStoreCap.Service
{
    public class MD5Service
    {
        public string Secret { get; set; }

        public MD5Service(string secret)
        {
            this.Secret = secret;
        }

        public string Enc(string url)
        {
            var uri = new Uri(url);

            var query = uri.Query.TrimStart(new char[] { '?' });

            var keyValues = query.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var paraDic = new Dictionary<string, string>();
            keyValues.ForEach(s =>
            {
                var parts = s.Split('=');
                if (parts.Length == 2 && string.Compare(parts[0], "sign", true) != 0)
                {
                    paraDic[parts[0]] = parts[1];
                }
            });

            paraDic = paraDic.SortHightedByKey();

            var encSource = new StringBuilder();

            foreach (var keyvalue in paraDic)
            {
                encSource.Append(string.Format("{0}={1}&", keyvalue.Key, keyvalue.Value));
            }
            var keyString = encSource.ToString().TrimEnd('&');

            keyString = string.Format("{0}{1}", keyString, Secret);

            var ret = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                ret = GetMd5Hash(md5Hash, keyString);
            }

            return ret;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
