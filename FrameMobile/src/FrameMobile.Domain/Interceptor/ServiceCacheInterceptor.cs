using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;
using FrameMobile.Cache;
using Snap;
using StructureMap;

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

            var cacheKey = GetCacheKey(invocation);
            var redisCacheHepler = ObjectFactory.GetInstance<ICacheManagerHelper>();


            if (redisCacheHepler.Contains(cacheKey))
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
            else
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
    }
}
