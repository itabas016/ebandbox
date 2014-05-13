using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCore;

namespace FrameMobile.Core
{
    public class EncryptionHelper
    {
        public static Dictionary<char, char> Dic
        {
            get
            {
                if (_dic == null)
                {
                    _dic = new Dictionary<char, char>();

                    for (int i = 0; i < EncryptionSettings.SOURCE.Length; i++)
                    {
                        _dic[EncryptionSettings.TARGET[i]] = EncryptionSettings.SOURCE[i];
                    }
                }

                return _dic;
            }

        } static Dictionary<char, char> _dic;

        public static string DecryptUrl(string originUrl)
        {
            if (!originUrl.IsNullOrEmpty())
            {
                var sb = new StringBuilder();
                for (int i = 0; i < originUrl.Length; i++)
                {
                    var target = originUrl[i];
                    if (Dic.ContainsKey(target)) target = Dic[target];

                    sb.Append(target);
                }

                return sb.ToString();
            }

            return originUrl;
        }

        public static string EncryptUrl(string originUrl)
        {
            var dic = new Dictionary<char, char>();

            for (int i = 0; i < EncryptionSettings.TARGET.Length; i++)
            {
                dic[EncryptionSettings.SOURCE[i]] = EncryptionSettings.TARGET[i];
            }

            if (!originUrl.IsNullOrEmpty())
            {
                var sb = new StringBuilder();
                for (int i = 0; i < originUrl.Length; i++)
                {
                    var target = originUrl[i];
                    if (Dic.ContainsKey(target)) target = dic[target];

                    sb.Append(target);
                }

                return sb.ToString();
            }

            return originUrl;
        }
    }
}
