using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
using NCore;

namespace FrameMobile.Model.ThirdPart
{
    public class TouTiaoParameter
    {
        #region Prop

        //随机数
        public int Nonce 
        {
            get
            {
                Random random = new Random();
                var value = random.Next(1, 10000);
                return value;
            }
        }

        //加密签名
        public string Signature
        {
            get
            {
                var value = GenerateSignature();
                return value;
            }
        }

        //时间戳
        public int Timestamp
        {
            get
            {
                TimeSpan span = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                var value = (int)span.TotalSeconds;
                return value;
            }
        }

        //合作方
        public string Partner
        {
            get
            {
                var value = ConfigKeys.TYD_NEWS_TOUTIAO_PARTNER.ConfigValue();
                return value;
            }
        }

        //Secure Key
        public string SecureKey
        {
            get
            {
                var value = ConfigKeys.TYD_NEWS_TOUTIAO_SECURE_KEY.ConfigValue();
                return value;
            }
        }

        //分类标签
        public string Category { get; set; }

        #endregion

        public TouTiaoParameter()
        {

        }

        protected string GenerateSignature()
        {
            var dicArray = new string[] { this.SecureKey, this.Timestamp.ToString(), this.Nonce.ToString() };
            var encryptStr = DicSortALG.ArraySort(dicArray);
            var result = Encrypt.SHA1_Encrypt(encryptStr);
            return result;
        }
    }
}
