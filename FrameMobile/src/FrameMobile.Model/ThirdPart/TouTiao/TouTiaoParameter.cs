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
        public int Nonce { get; set; }

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
        public string Timestamp
        {
            get
            {
                var value = System.DateTime.Now.ToString("yyyyMMdd");
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
            var dicArray = new string[] { this.SecureKey, this.Timestamp, this.Nonce.ToString() };
            var encryptStr = DicSortALG.ArraySort(dicArray);
            var result = Encrypt.SHA1_Encrypt(encryptStr);
            return result;
        }
    }
}
