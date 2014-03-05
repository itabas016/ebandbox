using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrameMobile.Common;
using NCore;
using Snap;

namespace FrameMobile.Domain
{
    public class ServiceCacheAttribute : MethodInterceptAttribute
    {
        private int _timeoutSecs = ConfigKeys.SERVICE_CACHE_TIMEOUT_SECONDS.ConfigValue().ToInt32();
        public int TimeoutSecs
        {
            get
            {
                return _timeoutSecs;
            }
            set
            {
                _timeoutSecs = value;
            }
        }

        public RedisClientManagerType ClientType
        {
            get { return _clientType; }
            set { _clientType = value; }
        }
        private RedisClientManagerType _clientType = RedisClientManagerType.MixedCache;
    }
}
