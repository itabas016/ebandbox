using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using FrameMobile.Core;
using NCore;

namespace FrameMobile.Model
{
    public class MobileParam
    {
        #region consts
        public const string Key_IMSI = "imsi";
        public const string Key_IMEI = "imei";
        public const string Key_SMSCode = "smsc";
        public const string Key_Batch = "batch";
        public const string Key_DesignHouse = "dh";
        public const string Key_Manufacturer = "mf";
        public const string Key_BrandModel = "mpm";
        public const string Key_HardwareMode = "mod";
        public const string Key_DateOfProduction = "tm";
        public const string Key_Resolution = "lcd";
        public const string Key_MCode = "mcode";
        public const string Key_SIMNo = "sim";
        public const string Key_HasGravity = "gs";
        public const string Key_Capacitive = "cap";
        public const string Key_LBS = "lbs";
        public const string Key_AppVer = "ver";
        public const string Key_NetworkType = "nt";
        public const string Key_OS = "os";
        public const string Key_SoftwareVersion = "pver";
        public const string Key_IsTest = "istest";
        public const string Key_Encoding = "encoding";
        public const string Key_LBS_MCC = "mcc";
        public const string Key_LBS_MNC = "mnc";
        public const string Key_LBS_LAC = "lac";
        public const string Key_LBS_CELLID = "cid";
        public const string Key_Channel = "ch";
        #endregion

        public MobileParam()
        {
        }

        public MobileParam(IRequestRepository requestRepo)
        {
            this._requestRepo = requestRepo;
        }

        protected IRequestRepository _requestRepo;

        protected Dictionary<string, string> _realValue = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private bool _hasAddedToDic = false;

        /// <summary>
        /// 软件版本
        /// </summary>
        public virtual string SoftwareVersion { get { return this.GetValue(Key_SoftwareVersion); } }

        /// <summary>
        /// IMEI号
        /// </summary>
        public virtual string IMEI { get { return this.GetValue(Key_IMEI); } }

        /// <summary>
        /// IMSI号
        /// </summary>
        public virtual string IMSI { get { return this.GetValue(Key_IMSI); } }

        /// <summary>
        /// 短信中心号码
        /// </summary>
        public virtual string SMSCode { get { return this.GetValue(Key_SMSCode); } }

        /// <summary>
        /// 芯片类型
        /// </summary>
        public virtual string Batch { get { return this.GetValue(Key_Batch); } }

        /// <summary>
        /// 设计公司
        /// </summary>
        [ViewField(DisplayName = "DesignHouse")]
        public virtual string DisignHouse { get { return this.GetValue(Key_DesignHouse); } }

        /// <summary>
        /// 生产厂商
        /// </summary>
        public virtual string Manufacturer { get { return this.GetValue(Key_Manufacturer); } }

        /// <summary>
        /// 品牌型号
        /// </summary>
        public virtual string BrandModel { get { return this.GetValue(Key_BrandModel); } }

        /// <summary>
        /// 硬件版本
        /// </summary>
        public virtual string FirmwareMode { get { return this.GetValue(Key_HardwareMode); } }

        /// <summary>
        /// 出厂日期
        /// e.g. 2012/2/9 8:03:00
        /// </summary>
        public virtual string DateOfProduction { get { return this.GetValue(Key_DateOfProduction); } }

        /// <summary>
        /// 分辨率
        /// </summary>
        public virtual string Resolution
        {
            get
            {
                var resolution = this.GetValue(Key_Resolution);

                if (!resolution.IsNullOrEmpty())
                {
                    resolution = resolution.Replace(ASCII.COMMA, ASCII.MULTIPLY);
                }

                return resolution;
            }
        }

        /// <summary>
        /// 码机号
        /// </summary>
        public virtual string MCode { get { return this.GetValue(Key_MCode); } }

        /// <summary>
        /// 第几张SIM卡 
        /// </summary>
        public virtual string SIMNo { get { return this.GetValue(Key_SIMNo); } }

        /// <summary>
        /// 是否带重力感应
        /// </summary>
        public virtual bool? HasGravity { get { return this.GetValue<bool>(Key_HasGravity); } }

        /// <summary>
        /// 是否带电容屏
        /// </summary>
        public virtual bool? HasCapacitive { get { return this.GetValue<bool>(Key_Capacitive); } }

        /// <summary>
        /// 是否源自测试
        /// </summary>
        public virtual bool? IsTest { get { return this.GetValue<bool>(Key_IsTest); } }

        /// <summary>
        /// 客户端系统内核
        /// </summary>
        public virtual string OS { get { return this.GetValue(Key_OS); } }

        /// <summary>
        /// 手机渠道号
        /// </summary>
        public virtual string Channel { get { return this.GetValue(Key_Channel); } }

        /// <summary>
        /// 当前网络连接类型
        /// 比如：wifi 3g 2g
        /// </summary>
        public virtual string NetworkType { get { return this.GetValue(Key_NetworkType); } }

        /// <summary>
        /// 基站信息
        /// Location Based Service
        /// e.g. 460:00:14145:26494
        /// </summary>
        public virtual string LBS
        {
            get
            {
                var lbs = this.GetValue(Key_LBS);
                if (lbs.IsNullOrEmpty())
                {
                    var mcc = this.GetValue(Key_LBS_MCC);
                    var mnc = this.GetValue(Key_LBS_MNC);
                    var lac = this.GetValue(Key_LBS_LAC);
                    var cellid = this.GetValue(Key_LBS_CELLID);

                    if (!mcc.EqualsOrdinalIgnoreCase(invalid_lbs)
                        && !mnc.EqualsOrdinalIgnoreCase(invalid_lbs)
                        && !lac.EqualsOrdinalIgnoreCase(invalid_lbs)
                        && !cellid.EqualsOrdinalIgnoreCase(invalid_lbs))
                    {

                        lbs = string.Format("{1}{0}{2}{0}{3}{0}{4}", ASCII.COLON, mcc, mnc, lac, cellid);

                        if (lbs.EqualsOrdinalIgnoreCase(null_lbs)) lbs = null;
                        if (lbs.EqualsOrdinalIgnoreCase(null_lbs2)) lbs = null;
                    }
                }
                return lbs;
            }
        }
        const string invalid_lbs = "-1";
        const string null_lbs = "0:0:0:0";
        const string null_lbs2 = ":::";
        
        /// <summary>
        /// 应用版本号
        /// </summary>
        public virtual string AppVersion { get { return this.GetValue(Key_AppVer); } }

        [ViewField(IsDisplay = false)]
        public virtual string RequestBody { get; set; }

        /// <summary>
        /// The encoding you want to use for content
        /// </summary>
        public virtual string Encoding { get { return this.GetValue(Key_Encoding); } }

        /// <summary>
        /// For Redis
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual string GetRealValue(string key)
        {
            if (!_hasAddedToDic)
            {
                _realValue[Key_IMSI] = this.IMSI;
                _realValue[Key_IMEI] = this.IMEI;
                _realValue[Key_SMSCode] = this.SMSCode;
                _realValue[Key_Batch] = this.Batch;
                _realValue[Key_DesignHouse] = this.DisignHouse;
                _realValue[Key_Manufacturer] = this.Manufacturer;
                _realValue[Key_BrandModel] = this.BrandModel;
                _realValue[Key_HardwareMode] = this.FirmwareMode;
                _realValue[Key_DateOfProduction] = this.DateOfProduction;
                _realValue[Key_Resolution] = this.Resolution;
                _realValue[Key_MCode] = this.MCode;
                _realValue[Key_SIMNo] = this.SIMNo;
                _realValue[Key_HasGravity] = GetStringFromNullableBool(this.HasGravity);
                _realValue[Key_Capacitive] = GetStringFromNullableBool(this.HasCapacitive);
                _realValue[Key_LBS] = this.LBS;
                _realValue[Key_AppVer] = this.AppVersion;
                _realValue[Key_OS] = this.OS;
                _realValue[Key_NetworkType] = this.NetworkType;
                _realValue[Key_SoftwareVersion] = this.SoftwareVersion;
                _realValue[Key_IsTest] = GetStringFromNullableBool(this.IsTest);
                _realValue[Key_Encoding] = this.Encoding;

                _hasAddedToDic = true;
            }

            if (_realValue.ContainsKey(key))
            {
                return _realValue[key];
            }

            return string.Empty;
        }

        private string GetStringFromNullableBool(bool? bVal)
        {
            return bVal.HasValue ? bVal.GetValueOrDefault().ToInt32().ToString() : string.Empty;
        }

        public virtual string GetValue(string key)
        {
            if (_requestRepo == null) return null;
            if (key.IsNullOrEmpty()) return null;

            var headerValue = default(string);
            var queryStringValue = default(string);

            if (_requestRepo.Header != null)
            {
                headerValue = _requestRepo.Header[key];
            }

            if (headerValue.IsNullOrEmpty() && _requestRepo.QueryString != null)
            {
                queryStringValue = _requestRepo.QueryString[key];
                if (!queryStringValue.IsNullOrEmpty())
                    headerValue = queryStringValue;
            }

            return headerValue;
        }

        private Nullable<T> GetValue<T>(string key)
            where T : struct, IConvertible
        {
            if (_requestRepo == null || key.IsNullOrEmpty()) return null;

            var value = GetValue(key);
            if (value.IsNullOrEmpty()) return null;

            var ret = default(T);
            var type = typeof(T);

            if (type == typeof(Boolean)) // 0 - false, 1 - true
            {
                ret = (T)Convert.ChangeType(value.ToInt32().ToBoolean(), typeof(T));
            }

            return ret;
        }

        public string ToString(bool withHeader)
        {
            return ResultBuilder.BuildInstance(this);
        }

        public override string ToString()
        {
            return ResultBuilder.BuildInstanceWithExcludedPropertyNames(this, _excludedPropertyNames, false);
        }

        private List<string> _excludedPropertyNames = new List<string>();
        /// <summary>
        /// for Cache
        /// the excluded names would be ignored for cache key
        /// </summary>
        /// <param name="excludedPropertyNames"></param>
        public void SetExcludedPropertyNames(List<string> excludedPropertyNames)
        {
            _excludedPropertyNames = excludedPropertyNames;
        }

        public string GetPlatformVersion(string info)
        {
            if (!info.IsNullOrEmpty() && info.StartsWith("1"))
            {
                if (info.Split(ASCII.MINUS_CHAR).Length > 2)
                    return info.Split(ASCII.MINUS_CHAR)[1].TrimStart('y', 'l');
            }

            return string.Empty;
        }
    }
}
