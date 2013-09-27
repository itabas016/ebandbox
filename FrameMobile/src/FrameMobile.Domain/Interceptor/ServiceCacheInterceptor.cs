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

            var cacheKey = GetCacheKey(invocation, parameters);
            var redisCacheHepler = ObjectFactory.GetInstance<ICacheManagerHelper>();

            if (redisCacheHepler.Contains(cacheKey))
            {
                GetDataByCacheKey(invocation, redisCacheHepler, parameters, cacheKey);
            }
            else
            {
                AddCacheKey(invocation, redisCacheHepler, parameters, svcCacheAttribute, cacheKey);
            }
        }

        private string GetCacheKey(IInvocation invocation)
        {
            StringBuilder paramSb = new StringBuilder();
            var parameters = invocation.Method.GetParameters();
            var args = invocation.Arguments;
            if (parameters != null && parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, args[i] == null ? string.Empty : args[i].ToString());
                }
            }

            var cachekey = string.Format("{0}|{1}|{2}", invocation.TargetType.Name, invocation.Method.Name, paramSb.ToString());

            return "SVC:" + cachekey.SHA1Hash();
        }

        private string GetCacheKey(IInvocation invocation, ParameterInfo[] parameters)
        {
            var methodName = invocation.Method.Name;
            var checkName = "GetNewsContentList";

            StringBuilder paramSb = new StringBuilder();
            var args = invocation.Arguments;
            if (parameters != null && parameters.Length > 0)
            {
                if (methodName == checkName)
                {
                    paramSb.Append(GetCacheKey(invocation, args, parameters));
                }
                else
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, args[i] == null ? string.Empty : args[i].ToString());
                    }
                }
                var cachekey = string.Format("{0}|{1}|{2}", invocation.TargetType.Name, invocation.Method.Name, paramSb.ToString());

                return "SVC:" + cachekey.SHA1Hash();
            }
            return string.Empty;
        }

        private string GetCacheKey(IInvocation invocation, object[] args, ParameterInfo[] parameters)
        {
            StringBuilder paramSb = new StringBuilder();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.Equals(typeof(MobileParam)))
                {
                    var mobileParam = args[i] as MobileParam;
                    if (mobileParam != null)
                    {
                        var width = mobileParam.Resolution.GetResolutionWidth();
                        var value = width > Const.NEWS_HD_RESOLUTION_WIDTH ? Const.NEWS_HD_RESOLUTION_WIDTH : Const.NEWS_NORMAL_RESOLUTION_WIDTH;
                        paramSb.AppendFormat("{0}[{1}]", MobileParam.Key_Resolution, value);
                    }
                }
                if (parameters[i].Name == "stamp")
                {
                    var stamp = args[i] as string;
                    var modulo = stamp.ToInt32() / 120;
                    if (modulo == 0)
                    {
                        paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, stamp);
                    }
                }

                paramSb.AppendFormat("{0}[{1}]", parameters[i].Name, args[i] == null ? string.Empty : args[i].ToString());
            }
            return paramSb.ToString();
        }

        private void AddCacheKey(IInvocation invocation, ICacheManagerHelper redisCacheHepler, ParameterInfo[] parameters, ServiceCacheAttribute svcCacheAttribute, string cacheKey)
        {
            invocation.Proceed();

            redisCacheHepler.AddNullableData(cacheKey, invocation.ReturnValue, svcCacheAttribute.TimeoutSecs);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef && invocation.Arguments[i] != null)
                {
                    redisCacheHepler.Add(cacheKey + parameters[i].Name, invocation.Arguments[i], svcCacheAttribute.TimeoutSecs);
                }
            }
        }

        private void GetDataByCacheKey(IInvocation invocation, ICacheManagerHelper redisCacheHepler, ParameterInfo[] parameters, string cacheKey)
        {
            invocation.ReturnValue = redisCacheHepler.GetNullableData(cacheKey, invocation.Method.ReturnType);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsByRef)
                {
                    object argVal = redisCacheHepler.GetData(cacheKey + parameters[i].Name, parameters[i].ParameterType.GetElementType());
                    invocation.SetArgumentValue(i, argVal);
                }
            }
        }
    }
}
