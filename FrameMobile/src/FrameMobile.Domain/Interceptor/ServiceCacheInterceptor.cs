using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;
using FrameMobile.Cache;
using FrameMobile.Common;
using FrameMobile.Model;
using Snap;
using StructureMap;
using NCore;
using FrameMobile.Domain.Service;

namespace FrameMobile.Domain
{
    public class ServiceCacheInterceptor : MethodInterceptor
    {
        public override void InterceptMethod(IInvocation invocation, MethodBase method, Attribute attribute)
        {
            var svcCacheAttribute = (ServiceCacheAttribute)attribute;
            var parameters = invocation.Method.GetParameters();

            if (svcCacheAttribute.TimeoutSecs == 0)
            {
                invocation.Proceed();
                return;
            }

            CacheKeyManage(invocation, parameters, svcCacheAttribute);
        }

        public void CacheKeyManage(IInvocation invocation, ParameterInfo[] parameters, ServiceCacheAttribute svcCacheAttribute)
        {
            var parameterString = GenerateParameterKey(invocation, parameters);
            var cacheKey = GenerateCacheKey(invocation, parameterString);

            var redisCacheService = RedisCacheServiceFactory(svcCacheAttribute);

            if (redisCacheService.Contains(cacheKey))
            {
                GetDataByCacheKey(invocation, redisCacheService, parameters, cacheKey);
            }
            else
            {
                AddCacheKey(invocation, redisCacheService, parameters, svcCacheAttribute, cacheKey);
            }
        }

        public IRedisCacheService RedisCacheServiceFactory(ServiceCacheAttribute svcCacheAttribute)
        {
            var clientType = svcCacheAttribute.ClientType;
            var redisCacheService = clientType.RedisCacheServiceFactory();
            return redisCacheService;
        }

        private string GenerateCacheKey(IInvocation invocation, string cacheKeyString)
        {
            var cachekey = string.Format("{0}|{1}|{2}", invocation.TargetType.Name, invocation.Method.Name, cacheKeyString);

            return "SVC:" + cachekey.SHA1Hash();
        }

        private string GenerateParameterKey(IInvocation invocation, ParameterInfo[] parameters)
        {
            var methodName = invocation.Method.Name.ToLower();
            var args = invocation.Arguments;

            StringBuilder paramSb = new StringBuilder();

            switch (methodName)
            {
                case Const.NEWS_METHOD_NAME_GETNEWSCONTENTVIEWLIST:
                case Const.NEWS_METHOD_NAME_GETNEWSCOLLECTIONVIEW:
                case Const.NEWS_HELPER_METHOD_GETOLDESTNEWSCONTENTVIEW:
                case Const.NEWS_HELPER_METHOD_GETLATESTNEWSCONTENTVIEW:
                case Const.NEWS_HELPER_METHOD_GETLOCALCONTENTVIEWLIST:
                    paramSb = NewsContentCacheKey(paramSb, args, parameters);
                    break;
                case Const.NEWS_METHOD_NAME_GETIMAGETYPEBYRESOLUTION:
                case Const.NEWS_METHOD_NAME_GETNEWSRADARVIEWLIST:
                    paramSb = NewsImageTypeCacheKey(paramSb, args, parameters);
                    break;
                case Const.NEWS_METHOD_NAME_GETEXTRARATIOBYCHANNEL:
                case Const.NEWS_METHOD_NAME_GETEXTRAAPPVIEWLIST:
                    paramSb = NewsMobileChannelCacheKey(paramSb, args, parameters);
                    break;
                case Const.COMMON_HELPER_METHOD_NAME_GETMOBILEPROPERTY:
                case Const.WALLPAPER_METHOD_NAME_GETWALLPAPERVIEWLIST:
                case Const.WALLPAPER_METHOD_NAME_GETWALLPAPERVIEWDETAIL:
                case Const.WALLPAPER_HELPER_METHOD_NAME_GETLATESTWALLPAPERVIEWLIST:
                case Const.WALLPAPER_HELPER_METHOD_NAME_GETHOTTESTWALLPAPERVIEWLIST:
                    paramSb = MobilePropertyCacheKey(paramSb, args, parameters);
                    break;
                default:
                    paramSb = CommonCacheKey(paramSb, args, parameters);
                    break;
            }
            return paramSb.ToString();
        }

        private StringBuilder CommonCacheKey(StringBuilder paramSb, object[] args, ParameterInfo[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, args[i] == null ? string.Empty : args[i].ToString());
                }
            }
            return paramSb;
        }

        private StringBuilder NewsContentCacheKey(StringBuilder paramSb, object[] args, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.Equals(typeof(MobileParam)))
                {
                    var mobileParam = args[i] as MobileParam;
                    if (mobileParam != null)
                    {
                        var width = mobileParam.Resolution.DefaultValue().GetResolutionWidth();
                        var value = width > Const.NEWS_HD_RESOLUTION_WIDTH ? Const.NEWS_HD_RESOLUTION_WIDTH : Const.NEWS_NORMAL_RESOLUTION_WIDTH;
                        paramSb.AppendFormat("{0}[{1}]", MobileParam.Key_Resolution, value);

                        var channel = mobileParam.Channel.DefaultValue().ToLower();
                        paramSb.AppendFormat("{0}[{1}]", MobileParam.Key_Channel, channel);
                    }
                    continue;
                }
                if (parameters[i].Name == "stamp")
                {
                    var stamp = (long)args[i];
                    paramSb.Append(CacheTimeKey(parameters[i].Name, stamp));
                    continue;
                }

                paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, args[i] == null ? string.Empty : args[i].ToString());
            }
            return paramSb;
        }

        private StringBuilder NewsImageTypeCacheKey(StringBuilder paramSb, object[] args, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.Equals(typeof(MobileParam)))
                {
                    var mobileParam = args[i] as MobileParam;
                    if (mobileParam != null)
                    {
                        var width = mobileParam.Resolution.DefaultValue().GetResolutionWidth();
                        paramSb.AppendFormat("{0}[{1}]", MobileParam.Key_Resolution, width);
                    }
                }
                paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, args[i] == null ? string.Empty : args[i].ToString());
            }
            return paramSb;
        }

        private StringBuilder NewsMobileChannelCacheKey(StringBuilder paramSb, object[] args, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.Equals(typeof(MobileParam)))
                {
                    var mobileParam = args[i] as MobileParam;
                    if (mobileParam != null)
                    {
                        var channel = mobileParam.Channel.DefaultValue().ToLower();
                        paramSb.AppendFormat("{0}[{1}]", MobileParam.Key_Channel, channel);
                    }
                }
                paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, args[i] == null ? string.Empty : args[i].ToString());
            }
            return paramSb;
        }

        private StringBuilder MobilePropertyCacheKey(StringBuilder paramSb, object[] args, ParameterInfo[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.Equals(typeof(MobileParam)))
                {
                    var mobileParam = args[i] as MobileParam;
                    if (mobileParam != null)
                    {
                        var brand = mobileParam.Manufacturer.DefaultValue().ToLower();
                        brand = brand == Const.BRAND_KOOBEE ? Const.BRAND_KOOBEE : Const.BRAND_PCBA;
                        var resolution = mobileParam.Resolution.DefaultValue().ToLower();
                        paramSb.AppendFormat("{0}[{1}]", MobileParam.Key_Manufacturer, brand);
                        paramSb.AppendFormat("{0}[{1}]", MobileParam.Key_Resolution, resolution);
                    }
                    continue;
                }
                paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, args[i] == null ? string.Empty : args[i].ToString());
            }
            return paramSb;
        }

        private string CacheTimeKey(string keyName, long stamp)
        {
            var currentTime = stamp.UTCStamp();

            var date = int.Parse(currentTime.ToString("yyyyMMdd"));

            var Hm = int.Parse(currentTime.ToString("HHmm"));

            var value = string.Empty;

            if ((Hm >= 700 && Hm <= 1000) || (Hm >= 1130 && Hm <= 1430) || (Hm >= 1730 && Hm <= 2030))
            {
                // 2minutes
                value = string.Format("{0}{1}", date, Hm / 2);
            }
            else if ((Hm > 1000 && Hm < 1130) || (Hm > 1430 && Hm < 1730))
            {
                //5minutes
                value = string.Format("{0}{1}", date, Hm / 5);
            }
            else if ((Hm > 2030 && Hm < 2400))
            {
                //10minutes
                value = string.Format("{0}{1}", date, Hm / 10);
            }
            else if ((Hm >= 0 && Hm <= 700))
            {
                //20minutes
                value = string.Format("{0}{1}", date, Hm / 20);
            }
            return string.Format("{0}[{1}]", keyName, value);
        }

        private void AddCacheKey(IInvocation invocation, IRedisCacheService redisCacheService, ParameterInfo[] parameters, ServiceCacheAttribute svcCacheAttribute, string cacheKey)
        {
            invocation.Proceed();

            redisCacheService.AddNullableData(cacheKey, invocation.ReturnValue, svcCacheAttribute.TimeoutSecs);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef && invocation.Arguments[i] != null)
                {
                    redisCacheService.Add(cacheKey + parameters[i].Name, invocation.Arguments[i], svcCacheAttribute.TimeoutSecs);
                }
            }
        }

        private void GetDataByCacheKey(IInvocation invocation, IRedisCacheService redisCacheService, ParameterInfo[] parameters, string cacheKey)
        {
            invocation.ReturnValue = redisCacheService.GetNullableData(cacheKey, invocation.Method.ReturnType);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                {
                    object argVal = redisCacheService.GetData(cacheKey + parameters[i].Name, parameters[i].ParameterType.GetElementType());
                    invocation.SetArgumentValue(i, argVal);
                }
            }
        }
    }
}
