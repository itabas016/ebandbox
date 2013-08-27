using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisMapper;

namespace FrameMobile.Domain.Service
{
    public interface ISaveInstance
    {
        void Add<T>() where T : IRedisModel;
        void Delete<T>() where T : IRedisModel;

    }
}
