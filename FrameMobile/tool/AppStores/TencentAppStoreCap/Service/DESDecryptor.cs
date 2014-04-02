using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TencentAppStoreCap.Service
{
    public class DESDecryptor
    {
        private const string TENCENT_IV = "s(2L@f!o";

        public static string Decrypt(byte[] decryptBytes, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Encoding.UTF8.GetBytes(TENCENT_IV);
                var DCSP = new TripleDESCryptoServiceProvider();
                DCSP.Mode = CipherMode.ECB;

                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(decryptBytes, 0, decryptBytes.Length);
                        cStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
