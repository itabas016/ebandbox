using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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

            var encSource = new StringBuilder();

            encSource.Append(query);
            encSource.Append(Secret);

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
