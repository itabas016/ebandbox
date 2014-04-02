using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Service
{
    public class MD5Service
    {
        public MD5Service(string secret)
        {
            this.Secret = secret;
        }

        public string Secret { get; set; }

        public string Enc(string url)
        {
            var uri = new Uri(url);

            var query = uri.Query.TrimStart(new char[] { '?' });

            var keyValues = query.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var paraDic = new Dictionary<string, string>();
            keyValues.ForEach(s =>
            {
                var parts = s.Split('=');
                if (parts.Length == 2 && string.Compare(parts[0], "api_sig", true) != 0)
                {
                    paraDic[parts[0].ToLower()] = parts[1].ToLower();
                }
            });

            var encSource = new StringBuilder();
            encSource.Append(Secret);
            foreach (var keyvalue in paraDic)
            {
                encSource.Append(keyvalue.Key);
                encSource.Append(keyvalue.Value);
            }

            var ret = string.Empty;
            using (MD5 md5Hash = MD5.Create())
            {
                ret = GetMd5Hash(md5Hash, encSource.ToString());
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
